namespace ShareMoreXP;

public class DartTrap
{
    public const int DamageItemClassId = 65739;
    public const string DamageItemClassName = "ammoDartIron";
    public const string XPName = "_xpFromDartTrapKill";

    public static bool ValidateItemClassId(int itemClassId)
    {
        return DamageItemClassId == itemClassId;
    }

    public static bool ValidateXPName(string xpName)
    {
        return xpName.Contains(XPName);
    }
}