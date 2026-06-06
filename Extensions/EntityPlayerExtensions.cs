namespace ShareMoreXP;

public static class EntityPlayerExtensions
{
    public static bool IsNullOrNotAlive(this EntityPlayer player)
    {
        return player is null || !player.IsAlive();
    }

    public static void ShowTooltip(this EntityPlayer? player, string text)
    {
        if (player is null || player is not EntityPlayerLocal localPlayer)
        {
            return;
        }

        GameManager.ShowTooltip(localPlayer, text, true);
    }
    
    public static void AddProgressionXP(this EntityPlayer player, int amount, string xpName, Progression.XPTypes xpType)
    {
        if (player.Progression == null)
        {
            return;
        }

        player.Progression.AddLevelExp(amount, xpName, xpType);
    }

    public static void AddTrapKillXP(this EntityPlayer player, int amount, TrapType type)
    {
        if (player.isEntityRemote)
        {
            NetPackageTrapXPClient.SetupAndSend(player.entityId, amount, type);
            return;
        }

        player.AddProgressionXP(amount, type.ToXPName(), Progression.XPTypes.Kill);
    }

    public static void AddSharedXP(this EntityPlayer player, int amount, Progression.XPTypes xpType)
    {
        if (player.isEntityRemote)
        {
            NetPackageShareXPClient.SetupAndSend(player.entityId, amount, xpType);
            return;
        }

        player.AddProgressionXP(amount, xpType.ToSharedXPName(), xpType);
    }

    public static void SendSharedXPToServer(this EntityPlayer player, int amount, Progression.XPTypes xpType)
    {
        NetPackageShareXPServer.SetupAndSend(player.entityId, amount, xpType);
    }
}