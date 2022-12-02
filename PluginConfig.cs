namespace LightBattery;

public class PluginConfig : IConfigSection
{
    [Description("Display % left")]
    public bool DisplayBatterieLeft { get; set; } = true;

    [Description("Batterie % (0.01 is 1%) lose per 0.1s")]
    public float BatterieLose { get; set; } = 0.001f;
}