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
    private int size = 16;
    private int borderSize = 1;
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
                for (int y = 0; y < size-borderSize; y++) { // Start from the second row
                    for (int x = borderSize; x < size-borderSize; x++) {
                        pixels[y * size + x] = surroundedPixels[y * size + x];
                    }
                }
            }
            if (up) { // Copy top row from surrounded
                for (int x = borderSize; x < size-borderSize; x++) {
                    pixels[size*size-x-1] = overridePixels[size*size-x-1]; // Copy top row
                }
            }
            
            if (left) { // Copy left most column from surrounded
                int start = down ? 0 : borderSize;
                int end = up ? size : size-borderSize;
                for (int y = start; y < end; y++) {
                    pixels[y * size] = overridePixels[y * size]; // Copy leftmost column
                }
            }
            if (right) {
                int start = down ? 0 : borderSize;
                int end = up ? size : size-borderSize;
                for (int y = start; y < end; y++) {
                    pixels[y * size + (size - 1)] = overridePixels[y * size + (size - 1)]; // Copy rightmost column
                }
            }
            Sprite sprite = saveSprite(pixels,path,spriteName,size,size);
            List<int> neighbors = getNeighborRules(up,left,down,right);
            addRule(ruleTile,neighbors,sprite);
            
        }

        // Other surrounded sprites
        for (int i = 0; i < 4; i++) {
            bool up = i < 2;
            bool left = i % 2 == 0;
            
            Color[] pixels = copyArray(surroundedPixels);
            int startX = left ? 0 : size-borderSize;
            int endX = left ? borderSize : size;
            int startY = !up ? 0 : size-borderSize;
            int endY = !up ? borderSize : size;

            for (int x = startX; x < endX; x++) {
                for (int y = startY; y < endY; y++) {
                    pixels[y*size + x] = lonePixels[y*size+x];
                }
            }
            string spriteName = tileName;
            if (up && left) {
                spriteName += "_UPLEFT";
            } else if (up && !left) {
                spriteName += "_UPRIGHT";
            } else if (!up && left) {
                spriteName += "_DOWNLEFT";
            } else {
                spriteName += "_DOWNRIGHT";
            }
            Sprite sprite = saveSprite(pixels,path,spriteName,size,size);
            List<int> neighbors = getNeighborRules(true,true,true,true,up_left:up&&left,up_right:up&&!left,down_left:!up&&left,down_right:!up&&!left);
            addRule(ruleTile,neighbors,sprite);
        }

        
        ruleTile.name = $"{tileName}";
        AssetDatabase.CreateAsset(ruleTile,Path.Combine(path,$"{ruleTile.name}.asset"));
        AssetDatabase.Refresh();
        
    }

    

    private void addRule(RuleTile ruleTile , List<int> neighbors, Sprite sprite) {
        RuleTile.TilingRule rule = new RuleTile.TilingRule();
        rule.m_ColliderType = Tile.ColliderType.Grid;
        rule.m_Sprites = new Sprite[1];
        rule.m_Sprites[0] = sprite;
        rule.m_Neighbors = neighbors;
        ruleTile.m_TilingRules.Add(rule);
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
}
