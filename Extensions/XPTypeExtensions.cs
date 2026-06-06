namespace ShareMoreXP;

public static class XPTypeExtensions
{
    public static float? ToPartyXPRadius(this Progression.XPTypes type) => type switch
    {
        Progression.XPTypes.Harvesting => ShareMoreXPMod.Config.PartyHarvestingXPRadius,
        Progression.XPTypes.Upgrading => ShareMoreXPMod.Config.PartyUpgradingXPRadius,
        Progression.XPTypes.Crafting => ShareMoreXPMod.Config.PartyCraftingXPRadius,
        Progression.XPTypes.Selling => ShareMoreXPMod.Config.PartySellingXPRadius,
        Progression.XPTypes.Looting => ShareMoreXPMod.Config.PartyLootingXPRadius,
        Progression.XPTypes.Repairing => ShareMoreXPMod.Config.PartyRepairingXPRadius,
        _ => null
    };

    public static float? ToPartyXPPercent(this Progression.XPTypes type) => type switch
    {
        Progression.XPTypes.Harvesting => ShareMoreXPMod.Config.PartyHarvestingXPPercent,
        Progression.XPTypes.Upgrading => ShareMoreXPMod.Config.PartyUpgradingXPPercent,
        Progression.XPTypes.Crafting => ShareMoreXPMod.Config.PartyCraftingXPPercent,
        Progression.XPTypes.Selling => ShareMoreXPMod.Config.PartySellingXPPercent,
        Progression.XPTypes.Looting => ShareMoreXPMod.Config.PartyLootingXPPercent,
        Progression.XPTypes.Repairing => ShareMoreXPMod.Config.PartyRepairingXPPercent,
        _ => null
    };

    public static int? ToPartyXPMinimumAmount(this Progression.XPTypes type) => type switch
    {
        Progression.XPTypes.Harvesting => ShareMoreXPMod.Config.PartyHarvestingXPMinimumAmount,
        Progression.XPTypes.Upgrading => ShareMoreXPMod.Config.PartyUpgradingXPMinimumAmount,
        Progression.XPTypes.Crafting => ShareMoreXPMod.Config.PartyCraftingXPMinimumAmount,
        Progression.XPTypes.Selling => ShareMoreXPMod.Config.PartySellingXPMinimumAmount,
        Progression.XPTypes.Looting => ShareMoreXPMod.Config.PartyLootingXPMinimumAmount,
        Progression.XPTypes.Repairing => ShareMoreXPMod.Config.PartyRepairingXPMinimumAmount,
        _ => null
    };

    public static string ToXPName(this Progression.XPTypes type) => type switch
    {
        Progression.XPTypes.Kill => "_xpFromKill",
        Progression.XPTypes.Harvesting => "_xpFromHarvesting",
        Progression.XPTypes.Upgrading => "_xpFromUpgrading",
        Progression.XPTypes.Crafting => "_xpFromCrafting",
        Progression.XPTypes.Selling => "_xpFromSelling",
        Progression.XPTypes.Quest => "_xpFromQuest",
        Progression.XPTypes.Looting => "_xpFromLooting",
        Progression.XPTypes.Party => "_xpFromParty",
        Progression.XPTypes.Repairing => "_xpFromRepairing",
        Progression.XPTypes.Debug => "_xpFromDebug",
        Progression.XPTypes.Max => "_xpMax",
        _ => "_xpOther"
    };

    public static string ToSharedXPName(this Progression.XPTypes type)
        => $"{type.ToXPName()}{Constants.SharedPartyXPNameSuffix}";

    public static bool IsSharingEnabled(this Progression.XPTypes type) => type switch
    {
        Progression.XPTypes.Harvesting => ShareMoreXPMod.Config.PartyHarvestingXPShared,
        Progression.XPTypes.Upgrading => ShareMoreXPMod.Config.PartyUpgradingXPShared,
        Progression.XPTypes.Crafting => ShareMoreXPMod.Config.PartyCraftingXPShared,
        Progression.XPTypes.Selling => ShareMoreXPMod.Config.PartySellingXPShared,
        Progression.XPTypes.Looting => ShareMoreXPMod.Config.PartyLootingXPShared,
        Progression.XPTypes.Repairing => ShareMoreXPMod.Config.PartyRepairingXPShared,
        _ => false
    };
}