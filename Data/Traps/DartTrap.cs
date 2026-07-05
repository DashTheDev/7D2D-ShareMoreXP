namespace ShareMoreXP;

public class DartTrap
{
    public const string DamageItemClassName = "ammoDartIron";
    public const string XPName = "_xpFromDartTrapKill";

    public static bool ValidateItemClassName(string name)
    {
        return DamageItemClassName == name;
    }

    public static bool ValidateXPName(string xpName)
    {
        return xpName.Contains(XPName);
    }
}