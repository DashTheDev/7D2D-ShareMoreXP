using DashTheDev.SDTD.ModCore;
using HarmonyLib;

namespace ShareMoreXP;

[HarmonyPatch(typeof(GameManager), nameof(GameManager.PlayerSpawnedInWorld))]
public class PlayerSpawnedPatch
{
    private static void Postfix(GameManager __instance, ClientInfo _cInfo, RespawnType _respawnReason, Vector3i _pos, int _entityId)
    {
        if (GeneralUtility.IsNotRunningOnServer())
        {
            return;
        }

        if (_respawnReason != RespawnType.EnterMultiplayer && _respawnReason != RespawnType.JoinMultiplayer)
        {
            return;
        }

        NetPackageSmxpConfigClient.SetupAndSend(_entityId);
    }
}