using System.Linq;

namespace ShareMoreXP;

public class BarbedWire
{
    public const int BlockType_DamageStageZero = 23613;
    public const int BlockType_DamageStageOne = 23614;
    public const int BlockType_DamageStageTwo = 23615;
    public static readonly int[] BlockTypes = [BlockType_DamageStageZero, BlockType_DamageStageOne, BlockType_DamageStageTwo];

    public const string BlockName_DamageStateZero = "barbedFence";
    public const string BlockName_DamageStateOne = "barbedFenceDamaged01";
    public const string BlockName_DamageStateTwo = "barbedFenceDamaged02";
    public static readonly string[] BlockNames = [BlockName_DamageStateZero, BlockName_DamageStateOne, BlockName_DamageStateTwo];

    public const string XPName = "_xpFromBarbedWireKill";

    public static bool ValidateBlockType(int blockType)
    {
        return BlockTypes.Contains(blockType);
    }
}