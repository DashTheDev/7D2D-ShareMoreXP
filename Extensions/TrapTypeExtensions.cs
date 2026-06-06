namespace ShareMoreXP;

public static class TrapTypeExtensions
{
    public static string ToXPName(this TrapType type) => type switch
    {
        TrapType.WoodSpikes => WoodSpikes.XPName,
        TrapType.IronSpikes => IronSpikes.XPName,
        TrapType.BarbedWire => BarbedWire.XPName,
        _ => "_xpOther"
    };
}