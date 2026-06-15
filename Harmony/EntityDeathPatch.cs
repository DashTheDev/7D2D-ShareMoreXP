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

        GeneralUtility.LogLine($"OnEntityDeath {{ ID: {__instance.entityId}, Name: {__instance.entityName} }}");
        GeneralUtility.LogLine($"lastDamageResponse.Source: {__instance.lastDamageResponse.Source != null}");

        if (__instance?.lastDamageResponse.Source == null)
        {
            return;
        }

        TrapType? killedByTrapType = __instance.lastDamageResponse.Source.ToTrapType();

        GeneralUtility.LogLine($"killedByTrapType: {killedByTrapType}");

        if (!killedByTrapType.HasValue || killedByTrapType.Value.IsElectrical())
        {
            return;
        }

        GeneralUtility.LogLine($"Going to calculate XP!");

        int baseXPAmountForEntity = EntityClass.list[__instance.entityClass].ExperienceValue;
        int xpAmount = (int)EffectManager.GetValue(PassiveEffects.ExperienceGain, __instance.inventory.holdingItemItemValue, baseXPAmountForEntity, __instance);
        string xpName = killedByTrapType.Value.ToXPName();
        SharedXPConfig xpConfig = ShareMoreXPMod.Config.NonElectricalTrapKilling;

        EntityPlayer[] recipientPlayers = XPUtility.GetRecipientPlayers(__instance.position, xpConfig);
        int adjustedXPAmount = XPUtility.GetAdjustedXPAmount(xpAmount, xpConfig, false, recipientPlayers.Length);

        GeneralUtility.LogLine($"Ready to distribute XP {{ baseXPAmountForEntity: {baseXPAmountForEntity}, xpAmount: {xpAmount}, xpName: {xpName}, adjustedAmount: {adjustedXPAmount} }}");

        foreach (EntityPlayer player in recipientPlayers)
        {
            GeneralUtility.LogLine($"Player!");

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