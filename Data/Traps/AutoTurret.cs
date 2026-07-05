namespace ShareMoreXP;

public class AutoTurret
{
    public const string DamageItemClassName = "ammo762mmBulletBall";
    public const string XPName = "_xpFromAutoTurretKill";

    public static bool ValidateItemClassName(string name)
    {
        return DamageItemClassName == name;
    }

    public static bool ValidateXPName(string xpName)
    {
        return xpName.Contains(XPName);
    }
}