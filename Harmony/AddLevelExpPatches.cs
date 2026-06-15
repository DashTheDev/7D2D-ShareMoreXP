using System.Collections.Generic;
using HarmonyLib;
using static ShareMoreXP.ShareMoreXPConfig;

namespace ShareMoreXP;

[HarmonyPatch(typeof(Progression), nameof(Progression.AddLevelExp))]
public class AddLevelExpPatches
{
    private static void Prefix(Progression __instance, ref int _exp, string _cvarXPName, Progression.XPTypes _xpType)
    {
        GeneralUtility.LogLine($"Progression.AddLevelExp {{ XP: {_exp}, XPName: {_cvarXPName}, Type: {_xpType}}}");

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
            Progression.XPTypes.Harvesting => ShareMoreXPMod.Config.Harvesting,
            Progression.XPTypes.Upgrading => ShareMoreXPMod.Config.Upgrading,
            Progression.XPTypes.Crafting => ShareMoreXPMod.Config.Crafting,
            Progression.XPTypes.Selling => ShareMoreXPMod.Config.Selling,
            Progression.XPTypes.Looting => ShareMoreXPMod.Config.Looting,
            Progression.XPTypes.Repairing => ShareMoreXPMod.Config.Repairing,
            _ => ShareMoreXPMod.Config.Killing
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
        GeneralUtility.LogLine($"Progression.AddLevelExp should have changed _exp to {sharingPlayerAdjustedXPAmount}");
    }

    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        bool patched = false;
        List<CodeInstruction> codes = [.. instructions];

        GeneralUtility.LogTranspilerBefore(nameof(AddLevelExpPatches), codes);

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

        GeneralUtility.LogLine($"{nameof(AddLevelExpPatches)} Transpiler patch {(patched ? "was" : "was NOT")} applied!");
        GeneralUtility.LogTranspilerAfter(nameof(AddLevelExpPatches), codes);

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