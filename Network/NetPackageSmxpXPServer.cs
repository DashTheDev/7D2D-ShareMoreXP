using DashTheDev.SDTD.ModCore;
using static ShareMoreXP.ShareMoreXPConfig;

namespace ShareMoreXP;

public class NetPackageSmxpXPServer : NetPackage
{
    public XPGainInfo XpInfo { get; private set; } = new();

    public override NetPackageDirection PackageDirection => NetPackageDirection.ToServer;
    public override int GetLength() => XpInfo.GetPackageLength();

    public NetPackageSmxpXPServer Setup(XPGainInfo xpInfo)
    {
        XpInfo = xpInfo;
        return this;
    }

    public override void read(PooledBinaryReader _reader)
    {
        XpInfo.Read(_reader);
    }

    public override void write(PooledBinaryWriter writer)
    {
        base.write(writer);
        XpInfo.Write(writer);
    }

    public override void ProcessPackage(World world, GameManager callbacks)
    {
        if (world == null || GeneralUtility.IsNotRunningOnServer())
        {
            return;
        }

        SharedXPConfig xpConfig = XpInfo.Type switch
        {
            Progression.XPTypes.Harvesting => ShareMoreXPMod.Instance.Config.Harvesting,
            Progression.XPTypes.Upgrading => ShareMoreXPMod.Instance.Config.Upgrading,
            Progression.XPTypes.Crafting => ShareMoreXPMod.Instance.Config.Crafting,
            Progression.XPTypes.Selling => ShareMoreXPMod.Instance.Config.Selling,
            Progression.XPTypes.Looting => ShareMoreXPMod.Instance.Config.Looting,
            Progression.XPTypes.Repairing => ShareMoreXPMod.Instance.Config.Repairing,
            _ => ShareMoreXPMod.Instance.Config.Killing
        };

        if (XpInfo.Name.Contains(Constants.SharedNonElectricalTrapXPNameSuffix))
        {
            return;
        }

        if (XpInfo.Name.Contains(Constants.SharedElectricalTrapXPNameSuffix))
        {
            xpConfig = ShareMoreXPMod.Instance.Config.ElectricalTrapKilling;
        }

        EntityPlayer? sharingPlayer = GameManager.Instance.World.GetEntity(XpInfo.EntityID) as EntityPlayer;

        if (sharingPlayer == null)
        {
            return;
        }

        EntityPlayer[] recipientPlayers = XPUtility.GetRecipientPlayers(sharingPlayer, xpConfig);
        int adjustedXPAmount = XPUtility.GetAdjustedXPAmount(XpInfo.BaseAmount, xpConfig, false, recipientPlayers.Length);

        foreach (EntityPlayer player in recipientPlayers)
        {
            if (player.entityId == sharingPlayer.entityId)
            {
                continue;
            }

            XPAdjustedGainInfo adjustedXpInfo = new(player.entityId, XpInfo.BaseAmount, adjustedXPAmount, XpInfo.Name, XpInfo.Type);

            if (!player.isEntityRemote)
            {
                player.AddXPInfoToProgression(adjustedXpInfo);
            }
            else
            {
                NetPackageSmxpXPClient.SetupAndSend(adjustedXpInfo);
            }
        }

       ShareMoreXPMod.Instance.Logger.LogLine("Received XP package from client!");
    }

    public static void SetupAndSend(XPGainInfo xpInfo)
    {
        NetPackageSmxpXPServer package = NetPackageManager.GetPackage<NetPackageSmxpXPServer>().Setup(xpInfo);
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(package);
       ShareMoreXPMod.Instance.Logger.LogLine("Sending XP package to server!");
    }
}