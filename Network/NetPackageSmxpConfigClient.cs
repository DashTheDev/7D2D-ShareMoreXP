namespace ShareMoreXP;

public class NetPackageSmxpConfigClient : NetPackage
{
    public ShareMoreXPConfig Config { get; private set; } = new();

    public override NetPackageDirection PackageDirection => NetPackageDirection.ToClient;
    public override int GetLength() => Config.GetPackageLength();

    public NetPackageSmxpConfigClient Setup()
    {
        Config = ShareMoreXPMod.Config;
        return this;
    }

    public override void read(PooledBinaryReader reader)
    {
        Config.Read(reader);
    }

    public override void write(PooledBinaryWriter writer)
    {
        base.write(writer);
        Config.Write(writer);
    }

    public override void ProcessPackage(World world, GameManager callbacks)
    {
        ShareMoreXPMod.Instance.SetConfigInstance(Config);
        GeneralUtility.LogLine("Received config from server!");
    }

    public static void SetupAndSend(int entityId)
    {
        NetPackageSmxpConfigClient package = NetPackageManager.GetPackage<NetPackageSmxpConfigClient>().Setup();
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(package, _attachedToEntityId: entityId);
        GeneralUtility.LogLine("Sending config to client!");
    }
}