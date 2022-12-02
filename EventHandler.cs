using MEC;

namespace LightBattery;

public class EventHandler
{
    public Dictionary<Player, CoroutineHandle> PlayersCoroutines = new();

    public PluginClass _plugin;

    public EventHandler()
    {
        Server.Get.Events.Player.PlayerChangeItemEvent += OnPlayerChangeItem;
        Server.Get.Events.Player.PlayerLeaveEvent += OnPlayerLeave;
        Server.Get.Events.Round.RoundRestartEvent += OnRoundRestart;

        _plugin = PluginClass.Instance;
    }

    private void OnPlayerLeave(PlayerLeaveEventArgs ev)
    {
        StopCoroutineFor(ev.Player);
    }

    private void OnRoundRestart()
    {
        Timing.KillCoroutines(PlayersCoroutines.Values.ToArray());
    }

    private void OnPlayerChangeItem(PlayerChangeItemEventArgs ev)
    {
        if (!ev.Allow) return;
        
        if (ev.NewItem.ID == (int)ItemType.Flashlight)
        {
            if (!PlayersCoroutines.ContainsKey(ev.Player))
                PlayersCoroutines.Add(ev.Player, Timing.RunCoroutine(DrainBattery(ev.NewItem, ev.Player)));

        }
        else if (ev.OldItem.ID == (int)ItemType.Flashlight)
        {
            StopCoroutineFor(ev.Player);
        }
    }

    private void StopCoroutineFor(Player player) 
    {
        if (PlayersCoroutines.TryGetValue(player, out CoroutineHandle coroutineHandler))
        {
            Timing.KillCoroutines(coroutineHandler);
            PlayersCoroutines.Remove(player);
        }
    }

    const string datakey = "battery";
    public IEnumerator<float> DrainBattery(SynapseItem falshLight, Player player)
    {
        float battery;
        float batteryLose = _plugin.Config.BatterieLose;
        string message = _plugin.Translation.ActiveTranslation.DisplayMessage;
        var cultur = _plugin.CultureInfo;
        int i = 0;

        if (!falshLight.ItemData.TryGetValue(datakey, out object value))
        {
            falshLight.ItemData.Add(datakey, value: 1);
            battery = 1;
        }
        else
        {
            battery = (float)value;
        }

        while (battery > 0)
        {
            i++;
            battery -= batteryLose;
            falshLight.ItemData[datakey] = battery;
            Timing.WaitForSeconds(waitTime: 0.1f);
            
            if (i == 20) //20 bc is 0.1s * 20 = 2s
            {
                player.GiveTextHint(string.Format(cultur, message, battery), duration: 2f);
                i = 0;
            }
        }

        yield break;
    }

}