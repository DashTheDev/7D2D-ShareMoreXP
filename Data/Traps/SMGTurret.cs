namespace ShareMoreXP;

public class SMGTurret
{
    public const string DamageItemClassName = "ammo9mmBulletBall";
    public const string XPName = "_xpFromSMGTurretKill";

    public static bool ValidateItemClassName(string name)
    {
        return DamageItemClassName == name;
    }

    public static bool ValidateXPName(string xpName)
    {
        return xpName.Contains(XPName);
    }
}