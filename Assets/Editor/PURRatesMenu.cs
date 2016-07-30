using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[InitializeOnLoad]
public class PURRatesMenu {

  const string WATER_TILE_PATH = "Assets/Prefabs/Sea.prefab";

  static GameObject seaTile;

  static PURRatesMenu() {
    seaTile = (GameObject) AssetDatabase.LoadAssetAtPath(WATER_TILE_PATH, typeof(GameObject));

    if (seaTile == null) {
      Debug.LogError("Could not find " + WATER_TILE_PATH);
    } else {
      Debug.Log("PURRates init : done.");
    }
  }

  [MenuItem("PURRates/New Map")]
  private static void CreateWaterMap()
  {
    if (seaTile == null) return;

    CreateMapDialog dialog = (CreateMapDialog) EditorWindow.GetWindow(typeof(CreateMapDialog));
    dialog.ShowDialog(OnCreateMap);
  }

  static void OnCreateMap(int width, int height) {
    GameObject root = GameObject.Find("Map");
    RectTransform rootTransform = root.GetComponent<RectTransform>();
    RectTransform seaTileTransform = seaTile.GetComponent<RectTransform>();    
    List<Vector2> generatedVectors = new List<Vector2>();


    float tileXGap = seaTileTransform.sizeDelta.x / 2; 
    float tileYGap = seaTileTransform.sizeDelta.y / 2;

    int xOffset = -width / 2;
    int yOffset = -height / 2;

    while (rootTransform.childCount > 0) {
      GameObject.DestroyImmediate(rootTransform.GetChild(0).gameObject);
    }

    for (int x = xOffset; x < width + xOffset; x++) {
      for (int y = yOffset; y < height + yOffset; y++) {
        generatedVectors.Add(new Vector2(tileXGap * (x - y), tileYGap * (x + y)));
      }
    }

    foreach (Vector2 pos in generatedVectors.OrderBy(p => -p.y)) {
      GameObject tile = (GameObject) PrefabUtility.InstantiatePrefab(seaTile);
      RectTransform tileTransform = tile.GetComponent<RectTransform>();
      tileTransform.SetParent(rootTransform);        
      tileTransform.localPosition = new Vector3(pos.x, pos.y, 0);
      tileTransform.localScale = new Vector3(1, 1, 1);
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
