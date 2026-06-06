using HarmonyLib;

namespace ShareMoreXP;

[HarmonyPatch(typeof(Progression), nameof(Progression.AddLevelExp))]
public class AddLevelExpPatch
{
    private static void Prefix(Progression __instance, int _exp, string _cvarXPName, Progression.XPTypes _xpType)
    {
        Utility.LogLine($"Progression.AddLevelExp {{ XP: {_exp}, XPName: {_cvarXPName}, Type: {_xpType}}}");

        // TODO: Read config from server rather than client
        if (!_xpType.IsSharingEnabled() || XPHasAlreadyBeenShared(_cvarXPName))
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

    private static bool XPHasAlreadyBeenShared(string xpName)
    {
        return xpName.Contains(Constants.SharedPartyXPNameSuffix);
    }
}