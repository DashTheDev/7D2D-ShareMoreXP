namespace ShareMoreXP;

public class NetPackageSmxpXPClient : NetPackage
{
    public XPAdjustedGainInfo XpInfo { get; private set; } = new();

    public override NetPackageDirection PackageDirection => NetPackageDirection.ToClient;
    public override int GetLength() => XpInfo.GetPackageLength();

    public NetPackageSmxpXPClient Setup(XPAdjustedGainInfo xpInfo)
    {
        XpInfo = xpInfo;
        return this;
    }

    public override void read(PooledBinaryReader _reader)
    {
        XpInfo.Read(_reader);
    }

    public override void write(PooledBinaryWriter writer)
    {
        base.write(writer);
        XpInfo.Write(writer);
    }

    public override void ProcessPackage(World world, GameManager callbacks)
    {
        if (world == null)
        {
            return;
        }

        EntityPlayer player = world.GetEntity(XpInfo.EntityID) as EntityPlayer;

        if (player == null)
        {
            return;
        }

        GeneralUtility.LogLine("Received XP package from server!");
        player.AddXPInfoToProgression(XpInfo);
    }

    public static void SetupAndSend(XPAdjustedGainInfo xpInfo)
    {
        NetPackageSmxpXPClient package = NetPackageManager.GetPackage<NetPackageSmxpXPClient>().Setup(xpInfo);
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(package, _attachedToEntityId: xpInfo.EntityID);
        GeneralUtility.LogLine("Sending XP package to client!");
    }
}