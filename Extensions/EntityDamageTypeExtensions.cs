namespace ShareMoreXP;

public static class EntityDamageTypeExtensions
{
    public static TrapType? ToTrapType(this EntityDamageType type) => type switch
    {
        EntityDamageType.WoodSpikes => TrapType.WoodSpikes,
        EntityDamageType.IronSpikes => TrapType.IronSpikes,
        EntityDamageType.BarbedWire => TrapType.BarbedWire,
        _ => null
    };
}