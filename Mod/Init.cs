using System.IO;
using System.Reflection;
using HarmonyLib;
using Newtonsoft.Json;

namespace ShareMoreXP;

public partial class ShareMoreXPMod : IModApi
{
    public static Mod ModInstance { get; private set; }
    public static ShareMoreXPConfig Config { get; private set; }
    public static bool IsDebug => Config is not null && Config.IsDebug;

    public void InitMod(Mod _modInstance)
    {
        ModInstance = _modInstance;
        Config = new ShareMoreXPConfig();
        LoadConfig();

        Harmony harmony = new(_modInstance.Name);
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    private void LoadConfig()
    {
        string path = Path.Combine(ModInstance.Path, "config.json");

        if (File.Exists(path))
        {
            Config = JsonConvert.DeserializeObject<ShareMoreXPConfig>(File.ReadAllText(path));
        }

        File.WriteAllText(path, JsonConvert.SerializeObject(Config, Formatting.Indented));
    }
}