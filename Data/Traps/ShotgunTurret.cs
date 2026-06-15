namespace ShareMoreXP;

public class ShotgunTurret
{
    public const int DamageItemClassId = 65678;
    public const string DamageItemClassName = "ammoShotgunShell";
    public const string XPName = "_xpFromShotgunTurretKill";

    public static bool ValidateItemClassId(int itemClassId)
    {
        return DamageItemClassId == itemClassId;
    }

    public static bool ValidateXPName(string xpName)
    {
        return xpName.Contains(XPName);
    }
}