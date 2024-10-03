using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUtils
{
    public static readonly float TILE_SIZE = 1f;
    public static readonly int MAX_ROOM_SIZE = 4096;
    public static string WALL_LAYER_NAME = "Wall";
    public static void deleteChildren(Transform container) {
        for (int i = 0; i < container.childCount; i++) {
            GameObject.Destroy(container.GetChild(i).gameObject);
        }
    }
}
