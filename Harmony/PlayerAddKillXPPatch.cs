using HarmonyLib;
using static ShareMoreXP.ShareMoreXPConfig;

namespace ShareMoreXP;

[HarmonyPatch(typeof(EntityPlayer), nameof(EntityPlayer.AddKillXP))]
public class PlayerAddKillXPPatch
{
    private static bool Prefix(EntityPlayer __instance, EntityAlive killedEntity, float xpModifier = 1f)
    {
        if (killedEntity?.lastDamageResponse.Source == null)
        {
            return true;
        }

        int baseXPAmountForEntity = EntityClass.list[killedEntity.entityClass].ExperienceValue;
        int xpAmount = (int)EffectManager.GetValue(PassiveEffects.ExperienceGain, killedEntity.inventory.holdingItemItemValue, baseXPAmountForEntity, killedEntity);
        string xpName = Constants.DefaultKillXPName;
        SharedXPConfig xpConfig = ShareMoreXPMod.Instance.Config.Killing;
        TrapType? killedByTrapType = killedEntity.lastDamageResponse.Source.ToTrapType();

        // ElectricalTrap kills are only processed on the server, so we can share authoritatively here
        if (killedByTrapType.HasValue && killedByTrapType.Value.IsElectrical())
        {
            xpName = killedByTrapType.Value.ToXPName();
            xpConfig = ShareMoreXPMod.Instance.Config.ElectricalTrapKilling;

            if (ShareMoreXPMod.Instance.Config.ElectricalTrapKilling.RespectAdvancedEngineeringPercent)
            {
                xpAmount = (int)(xpAmount * (double)xpModifier + 0.5);
            }

            EntityPlayer[] recipientPlayers = XPUtility.GetRecipientPlayers(__instance, xpConfig);

            foreach (EntityPlayer player in recipientPlayers)
            {
                bool isSharingPlayer = player.entityId == __instance.entityId;
                int adjustedXPAmount = XPUtility.GetAdjustedXPAmount(xpAmount, xpConfig, isSharingPlayer, recipientPlayers.Length);
                XPAdjustedGainInfo adjustedXpInfo = new(player.entityId, xpAmount, adjustedXPAmount, xpName, Progression.XPTypes.Kill);

                if (!player.isEntityRemote)
                {
                    player.AddXPInfoToProgression(adjustedXpInfo);
                }
                else
                {
                    NetPackageSmxpXPClient.SetupAndSend(adjustedXpInfo);
                }
            }

            return false;
        }

        __instance.AddProgressionXP(xpAmount, xpName, Progression.XPTypes.Kill);
        return false;
    }
}