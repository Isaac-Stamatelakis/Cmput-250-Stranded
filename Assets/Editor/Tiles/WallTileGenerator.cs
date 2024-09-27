using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.IO;

public class WallTileGenerator : EditorWindow {
    private Sprite loneSprite;
    private Sprite downLoneSprite;
    private Sprite surroundedSprite;
    private int wallHeight = 12;
    private int size = 16;
    private string tileName;
    [MenuItem("Tools/Tile/Wall")]
    public static void ShowWindow()
    {
        WallTileGenerator window = (WallTileGenerator)EditorWindow.GetWindow(typeof(WallTileGenerator));
        window.titleContent = new GUIContent("Wall Tile Generator");
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        loneSprite = EditorGUILayout.ObjectField("Lone Sprite", loneSprite, typeof(Sprite), true) as Sprite;
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        downLoneSprite = EditorGUILayout.ObjectField("Down Only Sprite", downLoneSprite, typeof(Sprite), true) as Sprite;
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        surroundedSprite = EditorGUILayout.ObjectField("Surrounded Sprite", surroundedSprite, typeof(Sprite), true) as Sprite;
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Size:", GUILayout.Width(70));
        size = EditorGUILayout.IntField(size);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Wall Height:", GUILayout.Width(70));
        wallHeight = EditorGUILayout.IntField(wallHeight);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Tile Name:", GUILayout.Width(70));
        tileName = EditorGUILayout.TextField(tileName);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        if (GUILayout.Button("Generate Tile Item"))
        {
            createRuleTile();
        }
    }

    void createRuleTile()
    {
        string path = EditorUtils.createAssetFolder(tileName);
        RuleTile ruleTile = ScriptableObject.CreateInstance<RuleTile>();

        Color[] lonePixels = loneSprite.texture.GetPixels();
        Color[] surroundedPixels = surroundedSprite.texture.GetPixels();
        Color[] downOnlyPixels = downLoneSprite.texture.GetPixels();
        

        if (lonePixels.Length != surroundedPixels.Length) {
            Debug.LogError("Provided sprites must be the same size!");
            return;
        }
        for (int direction = 0; direction < 16; direction++)
        {
            bool up = (direction & 8) != 0;
            bool left = (direction & 4) != 0;
            bool down = (direction & 2) != 0;
            bool right = (direction & 1) != 0;
            string spriteName = $"{tileName}_U_{up}_L_{left}_D_{down}_R_{right}";
            Color[] overridePixels = down ? surroundedPixels : downOnlyPixels;
            Color[] pixels = copyArray(lonePixels);
            if (down) { // Copy all pixels from surrounded except top row, left most column and right most column
                int start = right ? 0 : 1;
                int end = left ? size : size-1 ;
                for (int y = 0; y < size-1; y++) { // Start from the second row
                    for (int x = start; x < end; x++) {
                        pixels[y * size + x] = surroundedPixels[y * size + x];
                    }
                }
            }
            if (up) { // Copy top row from surrounded
                int start = right ? 0 : 1;
                int end = left ? size : size-1 ;
                for (int x = start; x < end; x++) {
                    pixels[size*size-x-1] = overridePixels[size*size-x-1]; // Copy top row
                }
            }
            if (left) { // Copy left most column from surrounded
                int start = !down ? 0 : 1;
                int end = !up ? size-1: size;
                for (int y = 1; y < size-1; y++) {
                    pixels[y * size] = overridePixels[y * size]; // Copy leftmost column
                }
            }
            if (right) {
                int start = !down ? 0 : 1;
                int end = !up ? size-1 : size;
                for (int y = 1; y < size-1; y++) {
                    pixels[y * size + (size - 1)] = overridePixels[y * size + (size - 1)]; // Copy rightmost column
                }
            }
            Sprite sprite = saveSprite(pixels,path,spriteName,size,size);
            List<int> neighbors = getNeighborRules(up,left,down,right);
            RuleTile.TilingRule rule = new RuleTile.TilingRule();
            rule.m_ColliderType = Tile.ColliderType.Grid;
            rule.m_Sprites = new Sprite[1];
            rule.m_Sprites[0] = sprite;
            rule.m_Neighbors = neighbors;
            ruleTile.m_TilingRules.Add(rule);
        }
        
        ruleTile.name = $"{tileName}";
        AssetDatabase.CreateAsset(ruleTile,Path.Combine(path,$"{ruleTile.name}.asset"));
        AssetDatabase.Refresh();
        
    }

    private Color[] copyArray(Color[] input) {
        Color[] pixels = new Color[input.Length];
        for (int i = 0; i < input.Length; i++) {
            pixels[i] = input[i];
        }
        return pixels;
    }

    private Sprite saveSprite(Color[] pixels, string spritePath, string spriteName, int width, int height) {
        Texture2D spliteTexture = new Texture2D(width,height);
        spliteTexture.SetPixels(0,0,width,height,pixels);
        

        string spriteSavePath = Path.Combine(spritePath,spriteName);
        byte[] pngBytes = spliteTexture.EncodeToPNG();
        File.WriteAllBytes(spriteSavePath+".png", pngBytes);
        AssetDatabase.Refresh();

        TextureImporter textureImporter = AssetImporter.GetAtPath(spriteSavePath + ".png") as TextureImporter;
        textureImporter.textureType = TextureImporterType.Sprite;
        textureImporter.spritePixelsPerUnit = 16;
        textureImporter.filterMode = FilterMode.Point;
        AssetDatabase.ImportAsset(spriteSavePath + ".png", ImportAssetOptions.ForceUpdate);
        AssetDatabase.Refresh();

        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spriteSavePath + ".png");
        return sprite;
    }

    private List<int> getNeighborRules(bool up, bool left, bool down, bool right, bool up_left=false , bool up_right = false, bool down_left = false, bool down_right = false) {
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
            neighborRules[0] = 1;
        }
        if (up_left) {
            neighborRules[2] = 1;
        }
        if (down_left) {
            neighborRules[5] = 1;
        }
        if (down_right) {
            neighborRules[7] = 1;
        }
        return neighborRules;
    }
}
