using static WaterSimulationApplyChanges.ChangesForChunk;

namespace ShareMoreXP;

public class ShareMoreXPConfig
{
    private const float DefaultXPRadius = 150f;
    private const float DefaultPartyXPPercent = 0.5f;
    private const int DefaultPartyXPMinimumAmount = 1;

    public bool IsEnabled { get; set; } = true;

#if DEBUG
    public bool IsDebug { get; set; } = true;
#else
    public bool IsDebug { get; set; }
#endif

    public bool DebugTranspilers { get; set; }
    public TrapsConfig Traps { get; set; } = new();
    public HarvestingConfig Harvesting { get; set; } = new();
    public UpgradingConfig Upgrading { get; set; } = new();
    public CraftingConfig Crafting { get; set; } = new();
    public SellingConfig Selling { get; set; } = new();
    public LootingConfig Looting { get; set; } = new();
    public RepairingConfig Repairing { get; set; } = new();

    public class TrapsConfig
    {
        public float XPRadius { get; set; } = DefaultXPRadius;
        public int XPAmount { get; set; } = 150;
        public bool XPSplitEvenly { get; set; } = true;
        public int XPSplitMinimumAmount { get; set; } = 5;
    }

    public abstract class SharedXPConfig
    {
        public bool XPShared { get; set; } = true;
        public float XPRadius { get; set; } = DefaultXPRadius;
        public float XPPercent { get; set; } = DefaultPartyXPPercent;
        public int XPMinimumAmount { get; set; } = DefaultPartyXPMinimumAmount;
    }

    public class HarvestingConfig : SharedXPConfig { }
    public class UpgradingConfig : SharedXPConfig { }
    public class CraftingConfig : SharedXPConfig { }
    public class SellingConfig : SharedXPConfig { }
    public class LootingConfig : SharedXPConfig { }
    public class RepairingConfig : SharedXPConfig { }

    private SharedXPConfig[] SharedXPConfigs => [Harvesting, Upgrading, Crafting, Selling, Looting, Repairing];

    public void Write(PooledBinaryWriter writer)
    {
        writer.Write(Traps.XPRadius);
        writer.Write(Traps.XPAmount);
        writer.Write(Traps.XPSplitEvenly);
        writer.Write(Traps.XPSplitMinimumAmount);

        foreach (SharedXPConfig sharedXPConfig in SharedXPConfigs)
        {
            writer.Write(sharedXPConfig.XPShared);
            writer.Write(sharedXPConfig.XPRadius);
            writer.Write(sharedXPConfig.XPPercent);
            writer.Write(sharedXPConfig.XPMinimumAmount);
        }
    }

    public void Read(PooledBinaryReader reader)
    {
        Traps.XPRadius = reader.ReadSingle();
        Traps.XPAmount = reader.ReadInt32();
        Traps.XPSplitEvenly = reader.ReadBoolean();
        Traps.XPSplitMinimumAmount = reader.ReadInt32();

        foreach (SharedXPConfig sharedXPConfig in SharedXPConfigs)
        {
            sharedXPConfig.XPShared = reader.ReadBoolean();
            sharedXPConfig.XPRadius = reader.ReadSingle();
            sharedXPConfig.XPPercent = reader.ReadSingle();
            sharedXPConfig.XPMinimumAmount = reader.ReadInt32();
        }
    }

    public static int GetPackageLength()
    {
        int trapConfigLength = Constants.PackageFloatLength + Constants.PackageIntLength + Constants.PackageBoolLength + Constants.PackageIntLength;
        int sharedXpConfigLength = Constants.PackageBoolLength + Constants.PackageFloatLength + Constants.PackageFloatLength + Constants.PackageIntLength;
        int sharedXpConfigLengths = sharedXpConfigLength * 6;
        return trapConfigLength + sharedXpConfigLengths;
    }
}