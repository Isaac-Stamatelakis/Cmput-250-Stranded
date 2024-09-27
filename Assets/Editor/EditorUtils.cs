using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


public static class EditorUtils
{
    private static readonly string SAVE_PATH = "Assets/EditorCreations";
    public static string createAssetFolder(string name) {
        string path = Path.Combine(SAVE_PATH,name);
        if (AssetDatabase.IsValidFolder(path)) {
            Debug.LogWarning("Replaced existing content at " + path);
            Directory.Delete(path,true);
        }
        AssetDatabase.CreateFolder("Assets/EditorCreations", name);
        AssetDatabase.Refresh();
        return path;
    }
}
