using System.Collections.Generic;
using HarmonyLib;

namespace ShareMoreXP;

[HarmonyPatch(typeof(Progression), nameof(Progression.AddLevelExp))]
public class AddLevelExpPatches
{
    private static void Prefix(Progression __instance, int _exp, string _cvarXPName, Progression.XPTypes _xpType)
    {
        Utility.LogLine($"Progression.AddLevelExp {{ XP: {_exp}, XPName: {_cvarXPName}, Type: {_xpType}}}");

        // TODO: Read config from server rather than client
        if (!_xpType.IsSharingEnabled() || XPHasBeenShared(_cvarXPName))
        {
            return;
        }

        EntityPlayer player = __instance.parent as EntityPlayer;

        if (player == null)
        {
            return;
        }

        Utility.ShareXPToParty(player, _exp, _xpType);
    }

    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        bool patched = false;
        List<CodeInstruction> codes = [.. instructions];

        Utility.LogTranspilerBefore(nameof(AddLevelExpPatches), codes);

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

        Utility.LogLine($"{nameof(AddLevelExpPatches)} Transpiler patch {(patched ? "was" : "was NOT")} applied!");
        Utility.LogTranspilerAfter(nameof(AddLevelExpPatches), codes);

        return codes;
    }

    private static bool XPHasBeenShared(string xpName)
    {
        return xpName.Contains(Constants.SharedPartyXPNameSuffix);
    }

    private static string GetXPIcon(string xpName)
    {
        if (XPHasBeenShared(xpName))
        {
            return "ui_game_symbol_shared_xp";
        }

        return "ui_game_symbol_xp";
    }
}