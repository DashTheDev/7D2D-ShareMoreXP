using System.Collections.Generic;

namespace ShareMoreXP;

public static class EntityDamageTracker
{
    private static readonly Dictionary<int, EntityDamageType> _entityIdToLatestDamageTypeDict = [];

    public static void UpsertLatestEntityDamageType(int entityID, EntityDamageType type)
    {
        if (!_entityIdToLatestDamageTypeDict.ContainsKey(entityID))
        {
            _entityIdToLatestDamageTypeDict.Add(entityID, type);
            return;
        }

        _entityIdToLatestDamageTypeDict[entityID] = type;
    }

    public static EntityDamageType? TryPopLatestEntityDamageType(int entityID)
    {
        if (!_entityIdToLatestDamageTypeDict.ContainsKey(entityID))
        {
            return null;
        }

        EntityDamageType type = _entityIdToLatestDamageTypeDict[entityID];
        _entityIdToLatestDamageTypeDict.Remove(entityID);
        return type;
    }
}