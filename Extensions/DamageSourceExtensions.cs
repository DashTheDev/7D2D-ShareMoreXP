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
            _ when WoodSpikes.ValidateBlockName(source.ItemClass.Name) => TrapType.WoodSpikes,
            _ when IronSpikes.ValidateBlockName(source.ItemClass.Name) => TrapType.IronSpikes,
            _ when BarbedWire.ValidateBlockName(source.ItemClass.Name) => TrapType.BarbedWire,
            _ when BladeTrap.ValidateItemClassName(source.ItemClass.Name) => TrapType.BladeTrap,
            _ when ShotgunTurret.ValidateItemClassName(source.ItemClass.Name) => TrapType.ShotgunTurret,
            _ when SMGTurret.ValidateItemClassName(source.ItemClass.Name) => TrapType.SMGTurret,
            _ when AutoTurret.ValidateItemClassName(source.ItemClass.Name) => TrapType.AutoTurret,
            _ => null
        };
    }
}