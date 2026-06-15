namespace ShareMoreXP;

public class SMGTurret
{
    public const int DamageItemClassId = 65669;
    public const string DamageItemClassName = "ammo9mmBulletBall";
    public const string XPName = "_xpFromSMGTurretKill";

    public static bool ValidateItemClassId(int itemClassId)
    {
        return DamageItemClassId == itemClassId;
    }

    public static bool ValidateXPName(string xpName)
    {
        return xpName.Contains(XPName);
    }
}