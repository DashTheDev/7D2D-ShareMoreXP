namespace ShareMoreXP;

public class NetPackageShareXPServer : NetPackageShareXPClient
{
    public override NetPackageDirection PackageDirection => NetPackageDirection.ToServer;

    public new NetPackageShareXPServer Setup(int entityId, int xpAmount, Progression.XPTypes xpType)
    {
        base.Setup(entityId, xpAmount, xpType);
        return this;
    }

    public override void ProcessPackage(World world, GameManager callbacks)
    {
        if (world == null)
        {
            return;
        }

        EntityPlayer player = world.GetEntity(EntityId) as EntityPlayer;

        if (player == null)
        {
            return;
        }

        Utility.LogLine("Received share XP package from client!");
        Progression.XPTypes xpType = (Progression.XPTypes)XpType;
        Utility.ShareXPToParty(player, XpAmount, xpType);
    }

    public static new void SetupAndSend(int entityId, int xpAmount, Progression.XPTypes xpType)
    {
        NetPackageShareXPServer package = NetPackageManager.GetPackage<NetPackageShareXPServer>().Setup(entityId, xpAmount, xpType);
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(package);
        Utility.LogLine("Sending share XP package to server!");
    }
}