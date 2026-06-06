using System.Linq;

namespace ShareMoreXP;

public class IronSpikes
{
    public const int BlockType_DamageStageZero = 23608;
    public const int BlockType_DamageStageOne = 23607;
    public const int BlockType_DamageStageTwo = 23606;
    public static readonly int[] BlockTypes = [BlockType_DamageStageZero, BlockType_DamageStageOne, BlockType_DamageStageTwo];

    public const string BlockName_DamageStateZero = "trapSpikesIronDmg0";
    public const string BlockName_DamageStateOne = "trapSpikesIronDmg1";
    public const string BlockName_DamageStateTwo = "trapSpikesIronDmg2";
    public static readonly string[] BlockNames = [BlockName_DamageStateZero, BlockName_DamageStateOne, BlockName_DamageStateTwo];

    public const string XPName = "_xpFromIronSpikesKill";

    public static bool ValidateBlockType(int blockType)
    {
        return BlockTypes.Contains(blockType);
    }
}