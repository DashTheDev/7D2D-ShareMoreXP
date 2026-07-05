using System.Linq;

namespace ShareMoreXP;

public class BarbedWire
{
    public const string BlockName_DamageStateZero = "barbedFence";
    public const string BlockName_DamageStateOne = "barbedFenceDamaged01";
    public const string BlockName_DamageStateTwo = "barbedFenceDamaged02";
    public static readonly string[] BlockNames = [BlockName_DamageStateZero, BlockName_DamageStateOne, BlockName_DamageStateTwo];

    public const string XPName = "_xpFromBarbedWireKill";

    public static bool ValidateBlockName(string name)
    {
        return BlockNames.Contains(name);
    }

    public static bool ValidateXPName(string xpName)
    {
        return xpName.Contains(XPName);
    }
}