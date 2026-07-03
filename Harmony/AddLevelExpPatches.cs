using System.Collections.Generic;
using DashTheDev.SDTD.ModCore;
using HarmonyLib;
using static ShareMoreXP.ShareMoreXPConfig;

namespace ShareMoreXP;

[HarmonyPatch(typeof(Progression), nameof(Progression.AddLevelExp))]
public class AddLevelExpPatches
{
    private static void Prefix(Progression __instance, ref int _exp, string _cvarXPName, Progression.XPTypes _xpType)
    {
       ShareMoreXPMod.Instance.Logger.LogLine($"Progression.AddLevelExp {{ XP: {_exp}, XPName: {_cvarXPName}, Type: {_xpType}}}");

        // XP has already been shared, no need to re-share it
        if (XPHasBeenShared(_cvarXPName))
        {
            return;
        }

        // Trap kills are shared via the server, no need to share
        if (XPIsNonElectricalTrapKill(_cvarXPName) || XPIsElectricalTrapKill(_cvarXPName))
        {
            return;
        }

        EntityPlayer? sharingPlayer = __instance.parent as EntityPlayer;

        if (sharingPlayer == null)
        {
            return;
        }

        SharedXPConfig xpConfig = _xpType switch
        {
            Progression.XPTypes.Harvesting => ShareMoreXPMod.Instance.Config.Harvesting,
            Progression.XPTypes.Upgrading => ShareMoreXPMod.Instance.Config.Upgrading,
            Progression.XPTypes.Crafting => ShareMoreXPMod.Instance.Config.Crafting,
            Progression.XPTypes.Selling => ShareMoreXPMod.Instance.Config.Selling,
            Progression.XPTypes.Looting => ShareMoreXPMod.Instance.Config.Looting,
            Progression.XPTypes.Repairing => ShareMoreXPMod.Instance.Config.Repairing,
            _ => ShareMoreXPMod.Instance.Config.Killing
        };

        _cvarXPName = $"{_cvarXPName}{Constants.SharedPartyXPNameSuffix}";

        EntityPlayer[] recipientPlayers = XPUtility.GetRecipientPlayers(sharingPlayer, xpConfig);
        int sharingPlayerAdjustedXPAmount = XPUtility.GetAdjustedXPAmount(_exp, xpConfig, true, recipientPlayers.Length);

        if (GeneralUtility.IsNotRunningOnServer())
        {
            XPGainInfo xpInfo = new(sharingPlayer.entityId, _exp, _cvarXPName, _xpType);
            NetPackageSmxpXPServer.SetupAndSend(xpInfo);
        }
        else
        {
            foreach (EntityPlayer player in recipientPlayers)
            {
                if (player.entityId == sharingPlayer.entityId || !player.isEntityRemote)
                {
                    continue;
                }

                int receivingPlayerAdjustedXPAmount = XPUtility.GetAdjustedXPAmount(_exp, xpConfig, false, recipientPlayers.Length);
                XPAdjustedGainInfo adjustedXpInfo = new(player.entityId, _exp, receivingPlayerAdjustedXPAmount, _cvarXPName, _xpType);
                NetPackageSmxpXPClient.SetupAndSend(adjustedXpInfo);
            }
        }

        _exp = sharingPlayerAdjustedXPAmount;
    }

    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        bool patched = false;
        List<CodeInstruction> codes = [.. instructions];

        ShareMoreXPMod.Instance.Logger.LogTranspilerBefore(nameof(AddLevelExpPatches), codes);

        for (int i = 0; i < codes.Count; i++)
        {
            if (codes[i].opcode != ReadableOpCodes.LoadString || codes[i].operand is not string iconValue || iconValue != "ui_game_symbol_xp")
            {
                continue;
            }

            List<CodeInstruction> replacementInstructions =
            [
                // Prepare arg1: _cvarXPName (caller method argument 2)
                new(ReadableOpCodes.LoadArgument2),

                // Call method: AddLevelExpPatches.GetXPIcon(xpName)
                new(ReadableOpCodes.CallMethod, AccessTools.Method(typeof(AddLevelExpPatches), nameof(GetXPIcon)))
            ];

            codes.RemoveAt(i);
            codes.InsertRange(i, replacementInstructions);

            patched = true;
            break;
        }

       ShareMoreXPMod.Instance.Logger.LogLine($"{nameof(AddLevelExpPatches)} Transpiler patch {(patched ? "was" : "was NOT")} applied!");
       ShareMoreXPMod.Instance.Logger.LogTranspilerAfter(nameof(AddLevelExpPatches), codes);

        return codes;
    }

    private static bool XPHasBeenShared(string xpName)
    {
        return xpName.Contains(Constants.SharedPartyXPNameSuffix);
    }

    private static bool XPIsTrapKill(string xpName)
    {
        return XPIsElectricalTrapKill(xpName) || XPIsNonElectricalTrapKill(xpName);
    }

    private static bool XPIsElectricalTrapKill(string xpName)
    {
        return xpName.Contains(Constants.SharedElectricalTrapXPNameSuffix);
    }

    private static bool XPIsNonElectricalTrapKill(string xpName)
    {
        return xpName.Contains(Constants.SharedNonElectricalTrapXPNameSuffix);
    }

    private static string GetXPIcon(string xpName)
    {
        if (XPHasBeenShared(xpName))
        {
            return Constants.SharedXPIcon;
        }

        if (XPIsTrapKill(xpName))
        {
            return Constants.TrapXPIcon;
        }

        return Constants.DefaultXPIcon;
    }
}