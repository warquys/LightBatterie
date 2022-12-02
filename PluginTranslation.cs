namespace LightBattery;

public class PluginTranslation : IPluginTranslation
{
    public string CultureInfo { get; set; } = "en-US";

    public string DisplayMessage { get; set; } = "Battery left {0:P}";

}