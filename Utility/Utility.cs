using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShareMoreXP;

public class Utility
{
    public static void LogLine(string str)
    {
        if (!ShareMoreXPMod.IsDebug)
        {
            return;
        }

        Log.Out($"[{ShareMoreXPMod.ModInstance.Name}](v{ShareMoreXPMod.ModInstance.VersionString}) {str}");
    }

    public static bool IsRunningOnServer()
    {
        if (SingletonMonoBehaviour<ConnectionManager>.Instance == null)
        {
            return false;
        }

        return SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer;
    }

    public static void GiveTrapKillXPToNearbyPlayers(Vector3 location, TrapType type)
    {
        List<EntityPlayer> nearbyPlayers = [];

        foreach (EntityPlayer player in GameManager.Instance.World.Players.list)
        {
            if (player.IsNullOrNotAlive() || player.Progression is null)
            {
                continue;
            }

            float distance = Vector3.Distance(location, player.position);

            if (distance > ShareMoreXPMod.Config.TrapXPRadius)
            {
                continue;
            }

            nearbyPlayers.Add(player);
        }

        int amount = ShareMoreXPMod.Config.TrapXPAmount;

        if (ShareMoreXPMod.Config.TrapXPSplitEvenly)
        {
            amount = Math.Max((int)(amount / (float)nearbyPlayers.Count), ShareMoreXPMod.Config.TrapXPSplitMinimumAmount);
        }

        foreach (EntityPlayer player in nearbyPlayers)
        {
            player.AddTrapKillXP(amount, type);
        }
    }

    public static void ShareXPToParty(EntityPlayer player, int amount, Progression.XPTypes xpType)
    {
        if (!player.IsInParty() || player.Party == null)
        {
            return;
        }

        if (!IsRunningOnServer())
        {
            player.SendSharedXPToServer(amount, xpType);
            return;
        }

        float? xpRadius = xpType.ToPartyXPRadius();
        float? xpPercent = xpType.ToPartyXPPercent();
        int? xpMinimumAmount = xpType.ToPartyXPMinimumAmount();

        if (xpRadius is null || xpPercent is null || xpMinimumAmount is null)
        {
            return;
        }

        amount = Math.Max((int)(amount * xpPercent), xpMinimumAmount.Value);

        foreach (EntityPlayer foundPlayer in GameManager.Instance.World.Players.list)
        {
            if (foundPlayer.entityId == player.entityId)
            {
                continue;
            }

            if (foundPlayer.IsNullOrNotAlive() || !foundPlayer.IsInParty() || foundPlayer.Party == null)
            {
                continue;
            }

            if (foundPlayer.Party.PartyID != player.Party.PartyID)
            {
                continue;
            }

            float distance = Vector3.Distance(player.position, foundPlayer.position);

            if (distance > xpRadius)
            {
                continue;
            }

            foundPlayer.AddSharedXP(amount, xpType);
        }
    }
}