namespace ShareMoreXP;

public static class XPTypeExtensions
{
    public static float? ToPartyXPRadius(this Progression.XPTypes type) => type switch
    {
        Progression.XPTypes.Harvesting => ShareMoreXPMod.Config.Harvesting.XPRadius,
        Progression.XPTypes.Upgrading => ShareMoreXPMod.Config.Upgrading.XPRadius,
        Progression.XPTypes.Crafting => ShareMoreXPMod.Config.Crafting.XPRadius,
        Progression.XPTypes.Selling => ShareMoreXPMod.Config.Selling.XPRadius,
        Progression.XPTypes.Looting => ShareMoreXPMod.Config.Looting.XPRadius,
        Progression.XPTypes.Repairing => ShareMoreXPMod.Config.Repairing.XPRadius,
        _ => null
    };

    public static float? ToPartyXPPercent(this Progression.XPTypes type) => type switch
    {
        Progression.XPTypes.Harvesting => ShareMoreXPMod.Config.Harvesting.XPPercent,
        Progression.XPTypes.Upgrading => ShareMoreXPMod.Config.Upgrading.XPPercent,
        Progression.XPTypes.Crafting => ShareMoreXPMod.Config.Crafting.XPPercent,
        Progression.XPTypes.Selling => ShareMoreXPMod.Config.Selling.XPPercent,
        Progression.XPTypes.Looting => ShareMoreXPMod.Config.Looting.XPPercent,
        Progression.XPTypes.Repairing => ShareMoreXPMod.Config.Repairing.XPPercent,
        _ => null
    };

    public static int? ToPartyXPMinimumAmount(this Progression.XPTypes type) => type switch
    {
        Progression.XPTypes.Harvesting => ShareMoreXPMod.Config.Harvesting.XPMinimumAmount,
        Progression.XPTypes.Upgrading => ShareMoreXPMod.Config.Upgrading.XPMinimumAmount,
        Progression.XPTypes.Crafting => ShareMoreXPMod.Config.Crafting.XPMinimumAmount,
        Progression.XPTypes.Selling => ShareMoreXPMod.Config.Selling.XPMinimumAmount,
        Progression.XPTypes.Looting => ShareMoreXPMod.Config.Looting.XPMinimumAmount,
        Progression.XPTypes.Repairing => ShareMoreXPMod.Config.Repairing.XPMinimumAmount,
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
        Progression.XPTypes.Harvesting => ShareMoreXPMod.Config.Harvesting.XPShared,
        Progression.XPTypes.Upgrading => ShareMoreXPMod.Config.Upgrading.XPShared,
        Progression.XPTypes.Crafting => ShareMoreXPMod.Config.Crafting.XPShared,
        Progression.XPTypes.Selling => ShareMoreXPMod.Config.Selling.XPShared,
        Progression.XPTypes.Looting => ShareMoreXPMod.Config.Looting.XPShared,
        Progression.XPTypes.Repairing => ShareMoreXPMod.Config.Repairing.XPShared,
        _ => false
    };
}