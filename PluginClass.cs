using System.Globalization;

namespace LightBattery;

[PluginInformation(
    Author = "VT",
    Name = "Light Battery",
    Description = "An exemple plugin for my frend lil-nespuique!",
    LoadPriority = 0,
    SynapseMajor = SynapseVersion.Major,
    SynapseMinor = SynapseVersion.Minor,
    SynapsePatch = SynapseVersion.Patch,
    Version = "v1.0.0"
    )]
public class PluginClass : AbstractPlugin
{
    public static PluginClass Instance { get; private set; }

    [Config(section = "Light Battery")]
    public PluginConfig Config { get; set; }
    
    [SynapseTranslation] 
    public SynapseTranslation<PluginTranslation> Translation { get; set; }
    public EventHandler EventHandler { get; set; }
    public CultureInfo  CultureInfo  { get; set; }

    public override void Load()
    {
        EventHandler = new EventHandler();

        Translation.AddTranslation(new PluginTranslation());
        Translation.AddTranslation(new PluginTranslation
        {
            CultureInfo = "fr-FR",
            DisplayMessage = "batterie restante {0:P}"
        }, language: "FRENCH");

        CultureInfo = new CultureInfo(Translation.ActiveTranslation.CultureInfo);

        base.Load();
    }

    public override void ReloadConfigs()
    {
        CultureInfo = new CultureInfo(Translation.ActiveTranslation.CultureInfo);
    }
}

