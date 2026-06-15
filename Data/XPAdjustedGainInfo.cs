namespace ShareMoreXP;

public class XPAdjustedGainInfo : XPGainInfo
{
    public int AdjustedAmount { get; private set; }

    public XPAdjustedGainInfo() { }

    public XPAdjustedGainInfo(int entityID, int baseAmount, int adjustedAmount, string name, Progression.XPTypes type) : base(entityID, baseAmount, name, type)
    {
        AdjustedAmount = adjustedAmount;
    }

    public override void Read(PooledBinaryReader reader)
    {
        base.Read(reader);
        AdjustedAmount = reader.ReadInt32();
    }

    public override void Write(PooledBinaryWriter writer)
    {
        base.Write(writer);
        writer.Write(AdjustedAmount);
    }

    public override int GetPackageLength()
    {
        return base.GetPackageLength() + Constants.PackageIntLength;
    }
}