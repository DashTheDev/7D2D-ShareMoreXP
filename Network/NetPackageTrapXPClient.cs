namespace ShareMoreXP;

public class NetPackageTrapXPClient : NetPackage
{
    public int EntityId { get; private set; }
    public int XpAmount { get; private set; }
    public short TrapType { get; private set; }

    public override NetPackageDirection PackageDirection => NetPackageDirection.ToClient;
    public override int GetLength() => 8;

    public NetPackageTrapXPClient Setup(int entityId, int xpAmount, TrapType trapType)
    {
        EntityId = entityId;
        XpAmount = xpAmount;
        TrapType = (short)trapType;
        return this;
    }

    public override void read(PooledBinaryReader _reader)
    {
        EntityId = _reader.ReadInt32();
        XpAmount = _reader.ReadInt32();
        TrapType = _reader.ReadInt16();
    }

    public override void write(PooledBinaryWriter writer)
    {
        base.write(writer);
        writer.Write(EntityId);
        writer.Write(XpAmount);
        writer.Write(TrapType);
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

        Utility.LogLine("Received trap kill XP package from server!");
        player.AddProgressionXP(XpAmount, ((TrapType)TrapType).ToXPName(), Progression.XPTypes.Kill);
    }

    public static void SetupAndSend(int entityId, int xpAmount, TrapType trapType)
    {
        NetPackageTrapXPClient package = NetPackageManager.GetPackage<NetPackageTrapXPClient>().Setup(entityId, xpAmount, trapType);
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(package, _attachedToEntityId: entityId);
        Utility.LogLine("Sending trap kill XP package to client!");
    }
}