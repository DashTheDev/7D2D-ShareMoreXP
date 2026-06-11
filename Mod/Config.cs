namespace ShareMoreXP;

public class ShareMoreXPConfig
{
    public bool IsEnabled = true;

#if DEBUG
    public bool IsDebug = true;
#else
    public bool IsDebug;
#endif

    public bool DebugTranspilers;

    private const float DefaultXPRadius = 150f;
    private const float DefaultPartyXPPercent = 0.5f;
    private const int DefaultPartyXPMinimumAmount = 1;

    public float TrapXPRadius = DefaultXPRadius;
    public int TrapXPAmount = 150;
    public bool TrapXPSplitEvenly = true;
    public int TrapXPSplitMinimumAmount = 5;

    public bool PartyHarvestingXPShared = true;
    public float PartyHarvestingXPRadius = DefaultXPRadius;
    public float PartyHarvestingXPPercent = DefaultPartyXPPercent;
    public int PartyHarvestingXPMinimumAmount = DefaultPartyXPMinimumAmount;

    public bool PartyUpgradingXPShared = true;
    public float PartyUpgradingXPRadius = DefaultXPRadius;
    public float PartyUpgradingXPPercent = DefaultPartyXPPercent;
    public int PartyUpgradingXPMinimumAmount = DefaultPartyXPMinimumAmount;

    public bool PartyCraftingXPShared = true;
    public float PartyCraftingXPRadius = DefaultXPRadius;
    public float PartyCraftingXPPercent = DefaultPartyXPPercent;
    public int PartyCraftingXPMinimumAmount = DefaultPartyXPMinimumAmount;

    public bool PartySellingXPShared = true;
    public float PartySellingXPRadius = DefaultXPRadius;
    public float PartySellingXPPercent = DefaultPartyXPPercent;
    public int PartySellingXPMinimumAmount = DefaultPartyXPMinimumAmount;

    public bool PartyLootingXPShared = true;
    public float PartyLootingXPRadius = DefaultXPRadius;
    public float PartyLootingXPPercent = DefaultPartyXPPercent;
    public int PartyLootingXPMinimumAmount = DefaultPartyXPMinimumAmount;

    public bool PartyRepairingXPShared = true;
    public float PartyRepairingXPRadius = DefaultXPRadius;
    public float PartyRepairingXPPercent = DefaultPartyXPPercent;
    public int PartyRepairingXPMinimumAmount = DefaultPartyXPMinimumAmount;
}