namespace ShareMoreXP;

public class XPGainInfo
{
    public int EntityID { get; private set; }
    public int BaseAmount { get; private set; }
    public string Name { get; private set; }
    public Progression.XPTypes Type { get; private set; }

    public XPGainInfo() { }
    public XPGainInfo(int entityID, int baseAmount, string name, Progression.XPTypes type)
    {
        EntityID = entityID;
        BaseAmount = baseAmount;
        Name = name;
        Type = type;
    }

    public virtual void Read(PooledBinaryReader reader)
    {
        EntityID = reader.ReadInt32();
        BaseAmount = reader.ReadInt32();
        Name = reader.ReadString();
        Type = (Progression.XPTypes)reader.ReadInt16();
    }

    public virtual void Write(PooledBinaryWriter writer)
    {
        writer.Write(EntityID);
        writer.Write(BaseAmount);
        writer.Write(Name);
        writer.Write((short)Type);
    }

    public virtual int GetPackageLength()
    {
        return Constants.PackageIntLength + Constants.PackageIntLength + Name.ToPackageLength() + Constants.PackageShortLength;
    }
}