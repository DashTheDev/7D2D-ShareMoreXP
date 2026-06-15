namespace ShareMoreXP;

public static class XPTypeExtensions
{
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
}