using HarmonyLib;
using static ShareMoreXP.ShareMoreXPConfig;

namespace ShareMoreXP;

[HarmonyPatch(typeof(EntityAlive), nameof(EntityAlive.OnEntityDeath))]
public class EntityDeathPatch
{
    [HarmonyPriority(Priority.Last)]
    static void Postfix(EntityAlive __instance)
    {
        if (GeneralUtility.IsNotRunningOnServer())
        {
            return;
        }

        if (__instance?.lastDamageResponse.Source == null)
        {
            return;
        }

        TrapType? killedByTrapType = __instance.lastDamageResponse.Source.ToTrapType();

        if (!killedByTrapType.HasValue || killedByTrapType.Value.IsElectrical())
        {
            return;
        }

        int baseXPAmountForEntity = EntityClass.list[__instance.entityClass].ExperienceValue;
        int xpAmount = (int)EffectManager.GetValue(PassiveEffects.ExperienceGain, __instance.inventory.holdingItemItemValue, baseXPAmountForEntity, __instance);
        string xpName = killedByTrapType.Value.ToXPName();
        SharedXPConfig xpConfig = ShareMoreXPMod.Config.NonElectricalTrapKilling;

        EntityPlayer[] recipientPlayers = XPUtility.GetRecipientPlayers(__instance.position, xpConfig);
        int adjustedXPAmount = XPUtility.GetAdjustedXPAmount(xpAmount, xpConfig, false, recipientPlayers.Length);

        foreach (EntityPlayer player in recipientPlayers)
        {
            XPAdjustedGainInfo xpInfo = new(player.entityId, xpAmount, adjustedXPAmount, xpName, Progression.XPTypes.Kill);

            if (!player.isEntityRemote)
            {
                player.AddXPInfoToProgression(xpInfo);
            }
            else
            {
                NetPackageSmxpXPClient.SetupAndSend(xpInfo);
            }
        }
    }
}