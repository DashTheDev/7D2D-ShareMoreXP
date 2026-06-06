namespace ShareMoreXP;

public class NetPackageShareXPClient : NetPackage
{
    public int EntityId { get; private set; }
    public int XpAmount { get; private set; }
    public short XpType { get; private set; }

    public override NetPackageDirection PackageDirection => NetPackageDirection.ToClient;
    public override int GetLength() => 8;

    public NetPackageShareXPClient Setup(int entityId, int xpAmount, Progression.XPTypes xpType)
    {
        EntityId = entityId;
        XpAmount = xpAmount;
        XpType = (short)xpType;
        return this;
    }

    public override void read(PooledBinaryReader _reader)
    {
        EntityId = _reader.ReadInt32();
        XpAmount = _reader.ReadInt32();
        XpType = _reader.ReadInt16();
    }

    public override void write(PooledBinaryWriter writer)
    {
        base.write(writer);
        writer.Write(EntityId);
        writer.Write(XpAmount);
        writer.Write(XpType);
    }

    public override void ProcessPackage(World world, GameManager callbacks)
    {
        if (world == null)
        {
            return;
        }

        EntityAlive entity = world.GetEntity(EntityId) as EntityAlive;

        if (entity == null)
        {
            return;
        }

        Utility.LogLine("Received share XP package from server!");
        Progression.XPTypes xpType = (Progression.XPTypes)XpType;
        entity.Progression.AddLevelExp(XpAmount, xpType.ToSharedXPName(), xpType, _instigatorID: EntityId);
    }

    public static void SetupAndSend(int entityId, int xpAmount, Progression.XPTypes xpType)
    {
        NetPackageShareXPClient package = NetPackageManager.GetPackage<NetPackageShareXPClient>().Setup(entityId, xpAmount, xpType);
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(package, _attachedToEntityId: entityId);
        Utility.LogLine("Sending share XP package to client!");
    }
}