namespace ShareMoreXP;

public static class DamageSourceExtensions
{
    public static EntityDamageType? ToEntityDamageType(this DamageSource source)
    {
        if (source.ItemClass == null)
        {
            return null;
        }

        return source.ItemClass.Id switch
        {
            _ when WoodSpikes.ValidateBlockType(source.ItemClass.Id) => EntityDamageType.WoodSpikes,
            _ when IronSpikes.ValidateBlockType(source.ItemClass.Id) => EntityDamageType.IronSpikes,
            _ when BarbedWire.ValidateBlockType(source.ItemClass.Id) => EntityDamageType.BarbedWire,
            _ => EntityDamageType.Other
        };
    }
}