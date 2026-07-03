using DashTheDev.SDTD.ModCore;

namespace ShareMoreXP;

public class ShareMoreXPConfig : XmlModConfig
{
    private const XPShareRecipientType DefaultRecipientType = XPShareRecipientType.Party;
    private const float DefaultProximityPenaltyPercentPerPlayer = 0.1f;
    private const float DefaultRadius = 150f;
    private const float DefaultFlatPercent = 0.5f;
    private const int DefaultMinimumAmount = 1;

    public KillingConfig Killing { get; set; } = new();
    public NonElectricalTrapKillingConfig NonElectricalTrapKilling { get; set; } = new();
    public ElectricalTrapKillingConfig ElectricalTrapKilling { get; set; } = new();
    public HarvestingConfig Harvesting { get; set; } = new();
    public UpgradingConfig Upgrading { get; set; } = new();
    public CraftingConfig Crafting { get; set; } = new();
    public SellingConfig Selling { get; set; } = new();
    public LootingConfig Looting { get; set; } = new();
    public RepairingConfig Repairing { get; set; } = new();

    public abstract class SharedXPConfig
    {
        public SharedXPConfig()
        {
            ShareMode = GetDefaultShareMode();
        }

        public XPShareMode ShareMode { get; set; }
        public XPShareRecipientType ShareRecipientType { get; set; } = DefaultRecipientType;
        public float ProximityPenaltyPercentPerPlayer { get; set; } = DefaultProximityPenaltyPercentPerPlayer;
        public float ShareRadius { get; set; } = DefaultRadius;
        public float FlatPercent { get; set; } = DefaultFlatPercent;
        public int MinimumAmount { get; set; } = DefaultMinimumAmount;

        protected virtual XPShareMode GetDefaultShareMode() => XPShareMode.ProximityPenalty;
    }

    public class KillingConfig : SharedXPConfig { }
    public class NonElectricalTrapKillingConfig : SharedXPConfig { }
    public class ElectricalTrapKillingConfig : SharedXPConfig
    {
        public bool RespectAdvancedEngineeringPercent { get; set; } = true;
    }

    public class HarvestingConfig : SharedXPConfig
    {
        protected override XPShareMode GetDefaultShareMode() => XPShareMode.FlatPercent;
    }

    public class UpgradingConfig : SharedXPConfig
    {
        protected override XPShareMode GetDefaultShareMode() => XPShareMode.FlatPercent;
    }

    public class CraftingConfig : SharedXPConfig
    {
        protected override XPShareMode GetDefaultShareMode() => XPShareMode.FlatPercent;
    }

    public class SellingConfig : SharedXPConfig
    {
        protected override XPShareMode GetDefaultShareMode() => XPShareMode.FlatPercent;
    }

    public class LootingConfig : SharedXPConfig
    {
        protected override XPShareMode GetDefaultShareMode() => XPShareMode.FlatPercent;
    }

    public class RepairingConfig : SharedXPConfig
    {
        protected override XPShareMode GetDefaultShareMode() => XPShareMode.FlatPercent;
    }

    private SharedXPConfig[] SharedXPConfigs => [Killing, NonElectricalTrapKilling, ElectricalTrapKilling, Harvesting, Upgrading, Crafting, Selling, Looting, Repairing];

    public void Write(PooledBinaryWriter writer)
    {
        foreach (SharedXPConfig sharedXPConfig in SharedXPConfigs)
        {
            writer.Write((short)sharedXPConfig.ShareMode);
            writer.Write((short)sharedXPConfig.ShareRecipientType);
            writer.Write(sharedXPConfig.ProximityPenaltyPercentPerPlayer);
            writer.Write(sharedXPConfig.ShareRadius);
            writer.Write(sharedXPConfig.FlatPercent);
            writer.Write(sharedXPConfig.MinimumAmount);

            if (sharedXPConfig is ElectricalTrapKillingConfig electricalTrapKillingConfig)
            {
                writer.Write(electricalTrapKillingConfig.RespectAdvancedEngineeringPercent);
            }
        }
    }

    public void Read(PooledBinaryReader reader)
    {
        foreach (SharedXPConfig sharedXPConfig in SharedXPConfigs)
        {
            sharedXPConfig.ShareMode = (XPShareMode)reader.ReadInt16();
            sharedXPConfig.ShareRecipientType = (XPShareRecipientType)reader.ReadInt16();
            sharedXPConfig.ProximityPenaltyPercentPerPlayer = reader.ReadSingle();
            sharedXPConfig.ShareRadius = reader.ReadSingle();
            sharedXPConfig.FlatPercent = reader.ReadSingle();
            sharedXPConfig.MinimumAmount = reader.ReadInt32();

            if (sharedXPConfig is ElectricalTrapKillingConfig electricalTrapKillingConfig)
            {
                electricalTrapKillingConfig.RespectAdvancedEngineeringPercent = reader.ReadBoolean();
            }
        }
    }

    public int GetPackageLength()
    {
        int electricalTrapConfigAdditionalLength = Constants.PackageBoolLength;
        int sharedXpConfigLength =
            Constants.PackageShortLength +
            Constants.PackageShortLength +
            Constants.PackageFloatLength + 
            Constants.PackageFloatLength + 
            Constants.PackageFloatLength + 
            Constants.PackageIntLength;
        return sharedXpConfigLength * SharedXPConfigs.Length + electricalTrapConfigAdditionalLength;
    }

    public void CopyFrom(ShareMoreXPConfig config)
    {
        Killing = config.Killing;
        NonElectricalTrapKilling = config.NonElectricalTrapKilling;
        ElectricalTrapKilling = config.ElectricalTrapKilling;
        Harvesting = config.Harvesting;
        Upgrading = config.Upgrading;
        Crafting = config.Crafting;
        Selling = config.Selling;
        Looting = config.Looting;
        Repairing = config.Repairing;
    }
}