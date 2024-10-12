using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.IO;

public class BroderRuleTileGenerator : EditorWindow {
    private Sprite sprite;
    private Color borderColor = Color.black;
    private string tileName;
    private static readonly int SIZE = 16;
    private static readonly int BORDER_SIZE = 1;
    private bool AnyTile = false;
    [MenuItem("Tools/Tile/Border")]
    public static void ShowWindow()
    {
        BroderRuleTileGenerator window = (BroderRuleTileGenerator)EditorWindow.GetWindow(typeof(BroderRuleTileGenerator));
        window.titleContent = new GUIContent("Wall Tile Generator");
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        sprite = EditorGUILayout.ObjectField("Lone Sprite", sprite, typeof(Sprite), true) as Sprite;
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        borderColor = EditorGUILayout.ColorField("Border Color", borderColor);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Tile Name:", GUILayout.Width(70));
        tileName = EditorGUILayout.TextField(tileName);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Connect Any", GUILayout.Width(120));
        AnyTile = EditorGUILayout.Toggle(AnyTile);
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
        RuleTile ruleTile = AnyTile ? ScriptableObject.CreateInstance<RuleTileConnectAny>() : ScriptableObject.CreateInstance<RuleTile>();
        Color[] defaultPixels = sprite.texture.GetPixels();
        
        for (int direction = 0; direction < 16; direction++)
        {
            bool up = (direction & 8) != 0;
            bool left = (direction & 4) != 0;
            bool down = (direction & 2) != 0;
            bool right = (direction & 1) != 0;
            string spriteName = $"{tileName}_U_{up}_L_{left}_D_{down}_R_{right}";
            Color[] pixels = copyArray(defaultPixels);
            if (!down) { // Copy all pixels from surrounded except top row, left most column and right most column
                for (int y = 0; y < BORDER_SIZE; y++) { // Start from the second row
                    for (int x = 0; x < SIZE; x++) {
                        pixels[y * SIZE + x] = borderColor;
                    }
                }
            }
            if (!up) { // Copy top row from surrounded
                for (int y = SIZE-BORDER_SIZE; y < SIZE; y++) { // Start from the second row
                    for (int x = 0; x < SIZE; x++) {
                        pixels[y * SIZE + x] = borderColor;
                    }
                }
            }
            
            if (!left) {
                for (int x = 0; x < BORDER_SIZE; x++) {
                    for (int y = 0; y < SIZE; y++) {
                        pixels[y * SIZE + x] = borderColor;
                    }
                }
            }
            if (!right) {
                for (int x = SIZE-BORDER_SIZE; x < SIZE; x++) {
                    for (int y = 0; y < SIZE; y++) {
                        pixels[y * SIZE + x] = borderColor;
                    }
                }
            }
            Sprite sprite = saveSprite(pixels,path,spriteName,SIZE,SIZE);
            bool diagUpRight = up && right && !down && !left;
            bool diagUpLeft = up && left && !down && !right;
            bool diagDownLeft = down && left && !up && !right;
            bool diagDownRight = down && right && !up && !left;
            List<int> neighbors = getNeighborRules(up,left,down,right);
            if (diagUpRight) {
                neighbors[2] = 1;
            }
            if (diagUpLeft) {
                neighbors[0] = 1;
            }
            if (diagDownLeft) {
                neighbors[5] = 1;
            }
            if (diagDownRight) {
                neighbors[7] = 1;
            }
            
            addRule(ruleTile,neighbors,sprite);
            
            if (!diagDownLeft && !diagUpLeft && !diagUpRight && !diagDownRight) {
                continue;
            }
            neighbors = getNeighborRules(up,left,down,right,up_left:diagUpLeft,up_right:diagUpRight,down_left:diagDownLeft,down_right:diagDownRight);
            spriteName += "_diag";
            if (diagUpRight) {
                for (int y = SIZE-BORDER_SIZE; y < SIZE; y++) {
                    for (int x = SIZE-BORDER_SIZE; x < SIZE; x++) {
                        pixels[y * SIZE + x] = borderColor;
                    }
                }
            }
            if (diagUpLeft) {
                for (int y = SIZE-BORDER_SIZE; y < SIZE; y++) {
                    for (int x = 0; x < BORDER_SIZE; x++) {
                        pixels[y * SIZE + x] = borderColor;
                    }
                }
            }
            if (diagDownLeft) {
                for (int y = 0; y < BORDER_SIZE; y++) {
                    for (int x = 0; x < BORDER_SIZE; x++) {
                        pixels[y * SIZE + x] = borderColor;
                    }
                }
            }
            if (diagDownRight) {
                for (int y = 0; y < BORDER_SIZE; y++) {
                    for (int x = SIZE-BORDER_SIZE; x < SIZE; x++) {
                        pixels[y * SIZE + x] = borderColor;
                    }
                }
            }
            Sprite sprite1 = saveSprite(pixels,path,spriteName,SIZE,SIZE);
            addRule(ruleTile,neighbors,sprite1);
            
        }
        ruleTile.name = $"{tileName}";
        AssetDatabase.CreateAsset(ruleTile,Path.Combine(path,$"{ruleTile.name}.asset"));
        AssetDatabase.Refresh();
        
    }

    private void saveRule(RuleTile ruleTile, Color[] pixels, string path, string spriteName, bool up, bool left, bool down, bool right) {
        
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
        textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
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
            neighborRules[2] = 2;
        }
        if (up_left) {
            neighborRules[0] = 2;
        }
        if (down_left) {
            neighborRules[5] = 2;
        }
        if (down_right) {
            neighborRules[7] = 2;
        }
        return neighborRules;
    }
}
