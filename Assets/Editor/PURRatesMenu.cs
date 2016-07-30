using System;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class PURRatesMenu {

  const string WATER_TILE_PATH = "Assets/Prefabs/WaterTile.prefab";

  static GameObject waterTile;

  static PURRatesMenu() {
    waterTile = (GameObject) AssetDatabase.LoadAssetAtPath(WATER_TILE_PATH, typeof(GameObject));

    if (waterTile == null) {
      Debug.LogError("Could not find " + WATER_TILE_PATH);
    } else {
      Debug.Log("PURRates init : done.");
    }
  }

  [MenuItem("PURRates/New Water Map")]
  private static void CreateWaterMap()
  {
    if (waterTile == null) return;

    CreateMapDialog dialog = (CreateMapDialog) EditorWindow.GetWindow(typeof(CreateMapDialog));
    dialog.ShowDialog(OnCreateMap);
  }

  static void OnCreateMap(int width, int height) {
    GameObject root = new GameObject();

    root.name = "Water";
    int xOffset = -width / 2;
    int zOffset = -height / 2;
  
    for (int x = xOffset; x < width + xOffset; x++) {
      for (int z = zOffset; z < height + zOffset; z++) {
        GameObject tile = (GameObject) PrefabUtility.InstantiatePrefab(waterTile);
        tile.transform.position = new Vector3(x, 0, z);
        tile.transform.SetParent(root.transform);
      }
    }
  }
}

class CreateMapDialog : EditorWindow {
 
  public delegate void OnCreateMap(int width, int height);

  OnCreateMap callback;
  string width;
  string height;

  public void ShowDialog(OnCreateMap callback) {
    this.callback = callback;
    Show();
  }

  void OnGUI() {
    width = EditorGUILayout.TextField("Map Width", width);
    height = EditorGUILayout.TextField("Map Height", height);

    if (GUILayout.Button("Create Map")) {
      OnClickCreateMap();
      GUIUtility.ExitGUI();
    }
  }

  void OnClickCreateMap() {
    width = width.Trim();
    height = height.Trim();

    callback(Int32.Parse(width), Int32.Parse(height));  
    Close();
  }
 
 }
