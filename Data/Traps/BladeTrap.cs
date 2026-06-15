namespace ShareMoreXP;

public class BladeTrap
{
    public const int DamageItemClassId = 19058;
    public const string DamageItemClassName = "bladeTrap";
    public const string XPName = "_xpFromBladeTrapKill";

    public static bool ValidateItemClassId(int itemClassId)
    {
        return DamageItemClassId == itemClassId;
    }

    public static bool ValidateXPName(string xpName)
    {
        return xpName.Contains(XPName);
    }
}