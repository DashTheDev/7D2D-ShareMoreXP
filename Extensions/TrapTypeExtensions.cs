namespace ShareMoreXP;

public static class TrapTypeExtensions
{
    public static bool IsNonElectrical(this TrapType type) => type switch
    {
        TrapType.WoodSpikes or
        TrapType.IronSpikes or
        TrapType.BarbedWire => true,
        _ => false
    };

    public static bool IsElectrical(this TrapType type)
    {
        return !type.IsNonElectrical();
    }

    public static string ToXPName(this TrapType type)
    {
        string xpName = type switch
        {
            TrapType.WoodSpikes => WoodSpikes.XPName,
            TrapType.IronSpikes => IronSpikes.XPName,
            TrapType.BarbedWire => BarbedWire.XPName,
            TrapType.BladeTrap => BladeTrap.XPName,
            TrapType.DartTrap => DartTrap.XPName,
            TrapType.ShotgunTurret => ShotgunTurret.XPName,
            TrapType.SMGTurret => SMGTurret.XPName,
            _ => Constants.DefaultKillXPName
        };

        string suffix = type.IsElectrical() ? Constants.SharedElectricalTrapXPNameSuffix : Constants.SharedNonElectricalTrapXPNameSuffix;
        return $"{xpName}{suffix}";
    }
}