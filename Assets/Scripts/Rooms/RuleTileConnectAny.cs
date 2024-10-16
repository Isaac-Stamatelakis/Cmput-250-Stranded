using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Any Connect Rule Tile", menuName = "Tiles/RuleTileAny")]
public class RuleTileConnectAny : RuleTile
{
    public override bool RuleMatch(int neighbor, TileBase other)
    {
        {
            switch (neighbor)
            {
                case TilingRuleOutput.Neighbor.This: return other != null;
                case TilingRuleOutput.Neighbor.NotThis: return other == null;
            }
            return true;
        }
    }

    public override bool RuleMatches(TilingRule rule, Vector3Int position, ITilemap tilemap, ref Matrix4x4 transform)
    {
        return base.RuleMatches(rule, position, tilemap, ref transform);
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject instantiatedGameObject)
    {
        return base.StartUp(position, tilemap, instantiatedGameObject);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
