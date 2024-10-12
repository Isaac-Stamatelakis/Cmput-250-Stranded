using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public class WallTopGenerator : EditorWindow
{
    private Sprite defaultSprite;
    private Sprite leftAndRight;
    private Sprite upAndDown;
    private Sprite downLeft;
    private Sprite upLeft;
    private Sprite upRight;
    private Sprite downRight;
    private string tileName;

    [MenuItem("Tools/Tile/WallTop")]
    public static void ShowWindow()
    {
        WallTopGenerator window = (WallTopGenerator)EditorWindow.GetWindow(typeof(WallTopGenerator));
        window.titleContent = new GUIContent("Wall Tile Generator");
        
    }

    void OnGUI()
    {

        FieldInfo[] fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(Sprite)) // Ensure it's a Sprite
            {
                // Add the field name and its value to the dictionary
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                field.SetValue(this, EditorGUILayout.ObjectField(field.Name, (Sprite)field.GetValue(this), typeof(Sprite), true) as Sprite);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
        }


        

        EditorGUILayout.BeginHorizontal();
        tileName = EditorGUILayout.TextField(tileName);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        if (GUILayout.Button("Generate Tile Item"))
        {
            generateRuleTile();
        }
    }

    private void generateRuleTile() {
        string path = EditorUtils.createAssetFolder(tileName);
        RuleTile ruleTile = ScriptableObject.CreateInstance<RuleTile>();
        Dictionary<(bool,bool,bool,bool), Sprite> spriteMap = new Dictionary<(bool, bool, bool, bool), Sprite>
        // up, down, left, right
        {
            { (true, false, true, false), upLeft },
            { (true, false, false, true), upRight },
            { (false, true, true, false), downLeft },
            { (false, true, false, true), downRight },
            { (false, false, true, true), leftAndRight },
            { (true, true, false, false), upAndDown },
        };
        for (int direction = 0; direction < 16; direction++)
        {
            bool up = (direction & 8) != 0;
            bool left = (direction & 4) != 0;
            bool down = (direction & 2) != 0;
            bool right = (direction & 1) != 0;
            (bool,bool,bool,bool) boolTuple = (up,down,left,right);
            Sprite sprite = spriteMap.ContainsKey(boolTuple) ? spriteMap[boolTuple] : defaultSprite;
            List<int> neighbors = TileEditorUtils.getNeighborRules(up,left,down,right);
            TileEditorUtils.addRule(ruleTile,neighbors,sprite);
        }
        ruleTile.name = $"{tileName}";
        TileEditorUtils.saveAsset(ruleTile,path);
    }
}
