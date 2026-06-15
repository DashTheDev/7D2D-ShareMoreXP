using System.Xml.Linq;
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
            Progression.XPTypes.Harvesting => ShareMoreXPMod.Config.Harvesting,
            Progression.XPTypes.Upgrading => ShareMoreXPMod.Config.Upgrading,
            Progression.XPTypes.Crafting => ShareMoreXPMod.Config.Crafting,
            Progression.XPTypes.Selling => ShareMoreXPMod.Config.Selling,
            Progression.XPTypes.Looting => ShareMoreXPMod.Config.Looting,
            Progression.XPTypes.Repairing => ShareMoreXPMod.Config.Repairing,
            _ => ShareMoreXPMod.Config.Killing
        };

        if (XpInfo.Name.Contains(Constants.SharedNonElectricalTrapXPNameSuffix))
        {
            return;
        }

        if (XpInfo.Name.Contains(Constants.SharedElectricalTrapXPNameSuffix))
        {
            xpConfig = ShareMoreXPMod.Config.ElectricalTrapKilling;
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

        GeneralUtility.LogLine("Received XP package from client!");
    }

    public static void SetupAndSend(XPGainInfo xpInfo)
    {
        NetPackageSmxpXPServer package = NetPackageManager.GetPackage<NetPackageSmxpXPServer>().Setup(xpInfo);
        SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(package);
        GeneralUtility.LogLine("Sending XP package to server!");
    }
}