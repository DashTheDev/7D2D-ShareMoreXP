namespace ShareMoreXP;

public class BladeTrap
{
    public const string DamageItemClassName = "bladeTrap";
    public const string XPName = "_xpFromBladeTrapKill";

    public static bool ValidateItemClassName(string name)
    {
        return DamageItemClassName == name;
    }

    public static bool ValidateXPName(string xpName)
    {
        return xpName.Contains(XPName);
    }
}