namespace ShareMoreXP;

public static class EntityPlayerExtensions
{
    public static bool IsNullOrNotAlive(this EntityPlayer player)
    {
        return player is null || !player.IsAlive();
    }

    public static bool IsInPartyWith(this EntityPlayer playerA, EntityPlayer playerB)
    {
        if (!playerA.IsInParty() || !playerB.IsInParty())
        {
            return false;
        }

        return playerA.Party.PartyID == playerB.Party.PartyID;
    }

    public static void AddXPInfoToProgression(this EntityPlayer player, XPAdjustedGainInfo xpInfo)
    {
        if (player.Progression == null)
        {
            return;
        }

        player.Progression.AddLevelExp(xpInfo.AdjustedAmount, xpInfo.Name, xpInfo.Type);
    }

    public static void AddProgressionXP(this EntityPlayer player, int amount, string xpName, Progression.XPTypes xpType)
    {
        if (player.Progression == null)
        {
            return;
        }

        player.Progression.AddLevelExp(amount, xpName, xpType);
    }
}