using System.Linq;

namespace ShareMoreXP;

public class IronSpikes
{
    public const string BlockName_DamageStateZero = "trapSpikesIronDmg0";
    public const string BlockName_DamageStateOne = "trapSpikesIronDmg1";
    public const string BlockName_DamageStateTwo = "trapSpikesIronDmg2";
    public static readonly string[] BlockNames = [BlockName_DamageStateZero, BlockName_DamageStateOne, BlockName_DamageStateTwo];

    public const string XPName = "_xpFromIronSpikesKill";

    public static bool ValidateBlockName(string name)
    {
        return BlockNames.Contains(name);
    }

    public static bool ValidateXPName(string xpName)
    {
        return xpName.Contains(XPName);
    }
}