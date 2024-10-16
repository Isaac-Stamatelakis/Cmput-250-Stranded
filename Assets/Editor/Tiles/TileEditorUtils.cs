using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.IO;

public static class TileEditorUtils
{
    public static void addRule(RuleTile ruleTile , List<int> neighbors, Sprite sprite) {
        RuleTile.TilingRule rule = new RuleTile.TilingRule();
        rule.m_ColliderType = Tile.ColliderType.Grid;
        rule.m_Sprites = new Sprite[1];
        rule.m_Sprites[0] = sprite;
        rule.m_Neighbors = neighbors;
        ruleTile.m_TilingRules.Add(rule);
    }

    public static List<int> getNeighborRules(bool up, bool left, bool down, bool right, bool up_left=false , bool up_right = false, bool down_left = false, bool down_right = false) {
        List<int> neighborRules = new List<int> {
            0,2,0,2,2,0,2,0
        };
        if (up) {
            neighborRules[1] = 1;
        }
        if (left) {
            neighborRules[3] = 1;
        }
        if (right) {
            neighborRules[4] = 1;
        }
        if (down) {
            neighborRules[6] = 1;
        }
        if (up_right) {
            neighborRules[2] = 0;
        }
        if (up_left) {
            neighborRules[0] = 0;
        }
        if (down_left) {
            neighborRules[5] = 0;
        }
        if (down_right) {
            neighborRules[7] = 0;
        }
        return neighborRules;
    }

    public static void saveAsset(ScriptableObject scriptableObject, string path) {
        AssetDatabase.CreateAsset(scriptableObject,Path.Combine(path,$"{scriptableObject.name}.asset"));
        AssetDatabase.Refresh();
    }
}
