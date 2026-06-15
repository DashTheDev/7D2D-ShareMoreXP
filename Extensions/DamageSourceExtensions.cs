namespace ShareMoreXP;

public static class DamageSourceExtensions
{
    public static TrapType? ToTrapType(this DamageSource source)
    {
        if (source.ItemClass == null)
        {
            return null;
        }

        return source.ItemClass.Id switch
        {
            _ when WoodSpikes.ValidateBlockType(source.ItemClass.Id) => TrapType.WoodSpikes,
            _ when IronSpikes.ValidateBlockType(source.ItemClass.Id) => TrapType.IronSpikes,
            _ when BarbedWire.ValidateBlockType(source.ItemClass.Id) => TrapType.BarbedWire,
            _ when BladeTrap.ValidateItemClassId(source.ItemClass.Id) => TrapType.BladeTrap,
            _ when ShotgunTurret.ValidateItemClassId(source.ItemClass.Id) => TrapType.ShotgunTurret,
            _ when SMGTurret.ValidateItemClassId(source.ItemClass.Id) => TrapType.SMGTurret,
            _ => null
        };
    }
}