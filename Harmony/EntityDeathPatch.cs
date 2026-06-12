using HarmonyLib;

namespace ShareMoreXP;

[HarmonyPatch(typeof(EntityAlive), nameof(EntityAlive.OnEntityDeath))]
public class EntityDeathPatch
{
    [HarmonyPriority(Priority.Last)]
    static bool Prefix(EntityAlive __instance)
    {
        if (Utility.IsNotRunningOnServer())
        {
            return true;
        }

        if (EntityDamageTracker.TryPopLatestEntityDamageType(__instance.entityId) is not EntityDamageType latestEntityDamageType)
        {
            return true;
        }

        if (latestEntityDamageType.ToTrapType() is not TrapType trapType)
        {
            return true;
        }

        Utility.GiveTrapKillXPToNearbyPlayers(__instance.position, trapType);
        return true;
    }
}