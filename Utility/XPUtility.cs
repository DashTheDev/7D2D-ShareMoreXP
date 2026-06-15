using System;
using System.Collections.Generic;
using UnityEngine;
using static ShareMoreXP.ShareMoreXPConfig;

namespace ShareMoreXP;

public class XPUtility
{
    public static int GetAdjustedXPAmount(int xpAmount, SharedXPConfig xpConfig, bool isSharer, int recipientCount)
    {
        if (xpConfig.ShareMode == XPShareMode.None)
        {
            return isSharer ? xpAmount : 0;
        }

        if (xpConfig.ShareMode == XPShareMode.FlatPercent)
        {
            xpAmount = isSharer ? xpAmount : (int)(xpAmount * xpConfig.FlatPercent);
        }
        else if (xpConfig.ShareMode == XPShareMode.SplitEvenly && recipientCount > 1)
        {
            xpAmount = (int)(xpAmount / (float)recipientCount);
        }
        else if (xpConfig.ShareMode == XPShareMode.ProximityPenalty && recipientCount > 1)
        {
            xpAmount = (int)(xpAmount * (1.0 - xpConfig.ProximityPenaltyPercentPerPlayer * (double)recipientCount));
        }

        return Math.Max(xpAmount, xpConfig.MinimumAmount);
    }

    public static EntityPlayer[] GetRecipientPlayers(EntityPlayer ownerPlayer, SharedXPConfig xpConfig)
    {
        List<EntityPlayer> recipientPlayers = [ownerPlayer];

        if (xpConfig.ShareMode == XPShareMode.None)
        {
            return recipientPlayers.ToArray();
        }

        if (xpConfig.ShareRecipientType == XPShareRecipientType.Party && !ownerPlayer.IsInParty())
        {
            return recipientPlayers.ToArray();
        }

        foreach (EntityPlayer player in GameManager.Instance.World.Players.list)
        {
            if (player.entityId == ownerPlayer.entityId || player.IsNullOrNotAlive())
            {
                continue;
            }

            if (xpConfig.ShareRecipientType == XPShareRecipientType.Allies && !ownerPlayer.IsFriendsWith(player))
            {
                continue;
            }

            if (xpConfig.ShareRecipientType == XPShareRecipientType.Party && !ownerPlayer.IsInPartyWith(player))
            {
                continue;
            }

            float distance = Vector3.Distance(ownerPlayer.position, player.position);

            if (distance > xpConfig.ShareRadius)
            {
                continue;
            }

            recipientPlayers.Add(player);
        }

        return recipientPlayers.ToArray();
    }

    // NOTE: Without an owner player, XPShareRecipientType.All is the only type supported
    public static EntityPlayer[] GetRecipientPlayers(Vector3 radiusCentre, SharedXPConfig xpConfig)
    {
        List<EntityPlayer> recipientPlayers = [];

        if (xpConfig.ShareMode == XPShareMode.None)
        {
            return recipientPlayers.ToArray();
        }

        foreach (EntityPlayer player in GameManager.Instance.World.Players.list)
        {
            if (player.IsNullOrNotAlive())
            {
                continue;
            }

            float distance = Vector3.Distance(radiusCentre, player.position);

            if (distance > xpConfig.ShareRadius)
            {
                continue;
            }

            recipientPlayers.Add(player);
        }

        return recipientPlayers.ToArray();
    }
}