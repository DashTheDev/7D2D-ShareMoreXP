using HarmonyLib;

namespace ShareMoreXP;

[HarmonyPatch(typeof(EntityAlive), nameof(EntityAlive.DamageEntity))]
public class DamageEntityPatch
{
    private static void Prefix(EntityAlive __instance, DamageSource _damageSource)
    {
        if (Utility.IsNotRunningOnServer())
        {
            return;
        }

        if (_damageSource.ToEntityDamageType() is not EntityDamageType entityDamageType)
        {
            return;
        }

        EntityDamageTracker.UpsertLatestEntityDamageType(__instance.entityId, entityDamageType);
    }
}