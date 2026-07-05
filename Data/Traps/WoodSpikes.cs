using System.Linq;

namespace ShareMoreXP;

public class WoodSpikes
{
    public const string BlockName_DamageStateZero = "trapSpikesWoodDmg0";
    public const string BlockName_DamageStateOne = "trapSpikesWoodDmg1";
    public const string BlockName_DamageStateTwo = "trapSpikesWoodDmg2";
    public static readonly string[] BlockNames = [BlockName_DamageStateZero, BlockName_DamageStateOne, BlockName_DamageStateTwo];

    public const string XPName = "_xpFromWoodSpikesKill";

    public static bool ValidateBlockName(string name)
    {
        return BlockNames.Contains(name);
    }

    public static bool ValidateXPName(string xpName)
    {
        return xpName.Contains(XPName);
    }
}