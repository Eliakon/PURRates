using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class MapConfig {
  public int width;
  public int height;
  public int islandsCount;
  public int minIslandWidth;
  public int maxIslandWidth;
  public int minIslandHeight;
  public int maxIslandHeight;
  public int xOffset;
  public int yOffset;

  public void ComputeOffsets() {
    xOffset = -width / 2;
    yOffset = -height / 2;
  }

  public void Save() {
    ComputeOffsets();
    EditorPrefs.SetInt("width", width);
    EditorPrefs.SetInt("height", height);
    EditorPrefs.SetInt("islandsCount", islandsCount);
    EditorPrefs.SetInt("minIslandWidth", minIslandWidth);
    EditorPrefs.SetInt("maxIslandWidth", maxIslandWidth);
    EditorPrefs.SetInt("minIslandHeight", minIslandHeight);
    EditorPrefs.SetInt("maxIslandHeight", maxIslandHeight);
  }

  public MapConfig() {
    this.width = EditorPrefs.GetInt("width", 20);
    this.height = EditorPrefs.GetInt("height", 20);
    this.islandsCount = EditorPrefs.GetInt("islandsCount", 3);
    this.minIslandWidth = EditorPrefs.GetInt("minIslandWidth", 6);
    this.maxIslandWidth = EditorPrefs.GetInt("maxIslandWidth", 8);
    this.minIslandHeight = EditorPrefs.GetInt("minIslandHeight", 6);
    this.maxIslandHeight = EditorPrefs.GetInt("maxIslandHeight", 8);    
  }

  public bool contains(int x, int y) {
    return x > xOffset && x < width + xOffset && y > yOffset && y < height + yOffset;
  }

}

public class TileConfig {
  public float x;
  public float y;
  public GameObject prefab;
  public float heat;
  public int mapX;
  public int mapY;

  public TileConfig(int mapX, int mapY, float x, float y, GameObject prefab) {
    this.mapX = mapX;
    this.mapY = mapY;
    this.heat = 0;
    this.x = x;
    this.y = y;
    this.prefab = prefab;
  }
}

public class IslandConfig {
  public float centerX;
  public float centerY;
  public int width;
  public int height;

  public IslandConfig(int width, int height) {
    this.width = width;
    this.height = height;
  }
}

[InitializeOnLoad]
public class PURRatesMenu {

  const string SEA_TILE_PATH = "Assets/Prefabs/Sea.prefab";
  const string SAND_TILE_PATH = "Assets/Prefabs/Sand.prefab";
  const string GRASS_TILE_PATH = "Assets/Prefabs/Grass.prefab";

  static GameObject seaTile;
  static GameObject sandTile;
  static GameObject grassTile;

  static GameObject root;
  static RectTransform rootTransform;
  static RectTransform seaTileTransform;

  static Game gameScript;

  static List<TileConfig> generatedTiles;


  static PURRatesMenu() {
    seaTile = loadPrefab(SEA_TILE_PATH);
    sandTile = loadPrefab(SAND_TILE_PATH);
    grassTile = loadPrefab(GRASS_TILE_PATH);

    gameScript = GameObject.Find("Game").GetComponent<Game>();

    root = GameObject.Find("Map");
    rootTransform = root.GetComponent<RectTransform>();
    seaTileTransform = seaTile.GetComponent<RectTransform>();
  }

  static GameObject loadPrefab(string path) {
    GameObject prefab = (GameObject) AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));

    if (prefab == null) {
      Debug.LogError("Could not find " + path);
    } else {
      Debug.Log("Load OK for " + path);
    }

    return prefab;
  }

  [MenuItem("PURRates/New Map")]
  private static void CreateWaterMap()
  {
    if (seaTile == null) return;

    CreateMapDialog dialog = (CreateMapDialog) EditorWindow.GetWindow(typeof(CreateMapDialog));
    dialog.ShowDialog(OnCreateMap);
  }

  static float easeIn (float t, float b, float c, float d) {
    t /= d;
    return c * t * t + b;
  }

  static List<IslandConfig> generateIslands(MapConfig mapConfig) {
    List<IslandConfig> islands = new List<IslandConfig>();

    for (int index = 0; index < mapConfig.islandsCount; ++index) {
      IslandConfig island = new IslandConfig(
        UnityEngine.Random.Range(mapConfig.minIslandWidth, mapConfig.maxIslandWidth),
        UnityEngine.Random.Range(mapConfig.minIslandHeight, mapConfig.maxIslandHeight)
      );

      int diameter = Mathf.Max(island.width, island.height);
      int ray = diameter / 2 + diameter % 2;

      foreach (TileConfig tile in generatedTiles.OrderBy(t => t.heat)) {
        if (tile.mapX > ray && tile.mapX + ray + 1 < mapConfig.width
          && tile.mapY > ray && tile.mapY + ray + 1 < mapConfig.height) {

          island.centerX = tile.mapX;
          island.centerY = tile.mapY;

          foreach (TileConfig heatTile in generatedTiles) {

            float lenX = Mathf.Abs(island.centerX - heatTile.mapX);
            float lenY = Mathf.Abs(island.centerY - heatTile.mapY);

            float distance = Mathf.Sqrt(Mathf.Pow(lenX, 2) + Mathf.Pow(lenY, 2));
          
            if (diameter * 3 - distance > 0) heatTile.heat += diameter * 3 - distance;
          }
          
          for (int islandX = (int) island.centerX - island.width / 2; islandX <= island.centerX + island.width / 2; ++islandX) {
            for (int islandY = (int) island.centerY - island.height / 2; islandY <= island.centerY + island.height / 2; ++islandY) {
              TileConfig toChange = generatedTiles.Find(t => t.mapX == islandX && t.mapY == islandY);
              
              if (Mathf.Abs(islandX - island.centerX) == island.width / 2 || Mathf.Abs(islandY - island.centerY) == island.height / 2) {
                toChange.prefab = sandTile;
              } else {
                toChange.prefab = grassTile;
              }
            }
          }

          break;
        }
      }

      islands.Add(island);
    }

    return islands;
  }

  static void OnCreateMap(MapConfig mapConfig) {
    float tileXGap = seaTileTransform.sizeDelta.x / 2;
    float tileYGap = seaTileTransform.sizeDelta.y / 2;

    generatedTiles = new List<TileConfig>();

    while (rootTransform.childCount > 0) {
      GameObject.DestroyImmediate(rootTransform.GetChild(0).gameObject);
    }

    for (int x = 0; x < mapConfig.width; ++x) {
      for (int y = 0; y < mapConfig.height; ++y) {
        int ox = x + mapConfig.xOffset;
        int oy = y + mapConfig.yOffset;

        generatedTiles.Add(
          new TileConfig(x, y, tileXGap * (ox - oy), tileYGap * (ox + oy), seaTile)
        );
      }
    }

    List<IslandConfig> islands = generateIslands(mapConfig);

    foreach (TileConfig tileConfig in generatedTiles.OrderBy(p => -p.y)) {
      GameObject tile = (GameObject) PrefabUtility.InstantiatePrefab(tileConfig.prefab);

      if (tileConfig.prefab == seaTile) {
        tile.GetComponent<SeaTile>().game = gameScript;
      }

      RectTransform tileTransform = tile.GetComponent<RectTransform>();
      tileTransform.SetParent(rootTransform);        
      tileTransform.localPosition = new Vector3(tileConfig.x, tileConfig.y, 0);
      tileTransform.localScale = new Vector3(1, 1, 1);
    }

  }
}

class CreateMapDialog : EditorWindow {
 
  public delegate void OnCreateMap(MapConfig mapConfig);

  OnCreateMap callback;
  MapConfig mapConfig = new MapConfig();

  public void ShowDialog(OnCreateMap callback) {
    this.callback = callback;
    Show();
  }

  void OnGUI() {
    mapConfig.width = EditorGUILayout.IntField("Map Width", mapConfig.width);
    mapConfig.height = EditorGUILayout.IntField("Map Height", mapConfig.height);
    mapConfig.islandsCount = EditorGUILayout.IntField("Islands Count", mapConfig.islandsCount);
    mapConfig.minIslandWidth = EditorGUILayout.IntField("Min Island Width", mapConfig.minIslandWidth);
    mapConfig.maxIslandWidth = EditorGUILayout.IntField("Max Island Width", mapConfig.maxIslandWidth);
    mapConfig.minIslandHeight = EditorGUILayout.IntField("Min Island Height", mapConfig.minIslandHeight);
    mapConfig.maxIslandHeight = EditorGUILayout.IntField("Max Island Height", mapConfig.maxIslandHeight);

    if (GUILayout.Button("Create Map")) {
      mapConfig.Save();
      OnClickCreateMap();
      GUIUtility.ExitGUI();
    }
  }

  void OnClickCreateMap() {
    callback(mapConfig);
    Close();
  }
 
 }
