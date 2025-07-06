using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using Unity.EditorCoroutines.Editor;
#endif

[ExecuteInEditMode]
public class EditableGrassSpawner : MonoBehaviour
{
    [Tooltip("The Tilemap to scan")]
    public Tilemap tilemap;

    [System.Serializable]
    public struct GrassEntry
    {
        public GameObject prefab;
        [Tooltip("Relative chance weight (higher = more common)")]
        public float weight;
        [Tooltip("Fixed Y offset applied to this prefab")]
        public float yOffset;
    }

    [Tooltip("Grass prefabs with weights and Y-offsets")]
    public GrassEntry[] grassEntries;
    [Tooltip("Flower prefabs with weights and Y-offsets")]
    public GrassEntry[] flowerEntries;

    [Tooltip("Max random X/Z offset from tile center")]
    public Vector2 maxOffset = new Vector2(0.3f, 0.3f);

    [Tooltip("Number of grass instances to spawn per tile")]
    [Min(1)]
    public int amountPerTile = 1;

    [Tooltip("Number of flower tries per tile")]
    [Min(1)]
    public int flowerAmount = 1;

    [Tooltip("Noise scale for flowers (higher = larger noise features)")]
    public float flowerNoiseScale = 0.2f;

    [Tooltip("Threshold for Perlin noise to spawn flowers (0-1)")]
    [Range(0f, 1f)]
    public float flowerThreshold = 0.6f;

    [Tooltip("Parent object for spawned grass/flowers")]
    public Transform parent;

    [Tooltip("Which TileBase marks grass spots")]
    public TileBase grassTile;
    [Tooltip("Which TileBase marks light grass spots")]
    public TileBase lightGrassTile;

    [Header("Scene Management")]
    [Tooltip("Create non-serialized objects for editing (won't save to scene)")]
    public bool useNonSerializedObjects = true;

    [Tooltip("Save/Load grass data to ScriptableObject")]
    public GrassDataAsset grassDataAsset;

    // Guard to prevent overlapping spawn calls
    public bool _isSpawning = false;

    private List<Transform> _spawnedObjects = new List<Transform>();

    /// <summary>
    /// Starts the spawning process non-blocking.
    /// </summary>
    public void StartSpawnGrass()
    {
        if (_isSpawning)
        {
            Debug.Log("Spawn already in progress, skipping.");
            return;
        }

#if UNITY_EDITOR
        EditorCoroutineUtility.StartCoroutineOwnerless(SpawnGrassRoutine());
#else
        StartCoroutine(SpawnGrassRoutine());
#endif
    }

    public void SaveGrassData()
    {
#if UNITY_EDITOR
        if (grassDataAsset == null)
        {
            // Create new asset
            grassDataAsset = ScriptableObject.CreateInstance<GrassDataAsset>();
            string path = EditorUtility.SaveFilePanelInProject("Save Grass Data", "GrassData", "asset", "Save grass data");
            if (string.IsNullOrEmpty(path)) return;
            AssetDatabase.CreateAsset(grassDataAsset, path);
        }

        grassDataAsset.SaveData(parent);
        EditorUtility.SetDirty(grassDataAsset);
        AssetDatabase.SaveAssets();
        Debug.Log("Grass data saved to asset!");
#endif
    }

    public void LoadGrassData()
    {
        if (grassDataAsset == null)
        {
            Debug.LogError("No grass data asset assigned!");
            return;
        }

        ClearAllGrass();
        grassDataAsset.LoadData(parent, useNonSerializedObjects);
        Debug.Log("Grass data loaded from asset!");
    }

    public void ClearAllGrass()
    {
        if (parent == null) return;

        // Clear spawned objects list
        _spawnedObjects.Clear();

        // Clear all children
        while (parent.childCount > 0)
        {
            var child = parent.GetChild(0);
#if UNITY_EDITOR
            DestroyImmediate(child.gameObject);
#else
            Destroy(child.gameObject);
#endif
        }
    }

    private System.Collections.IEnumerator SpawnGrassRoutine()
    {
        if (tilemap == null || grassEntries.Length == 0 || grassTile == null)
        {
            Debug.LogWarning("Missing required references for grass spawning.");
            yield break;
        }

        _isSpawning = true;

        if (parent == null) parent = transform;

        int round = 0;

        // Clear previous grass/flowers
        ClearAllGrass();

        // Compute weights
        float totalGrassWeight = 0f;
        foreach (var e in grassEntries) totalGrassWeight += e.weight;
        float totalFlowerWeight = 0f;
        foreach (var f in flowerEntries) totalFlowerWeight += f.weight;

        var bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                var cell = new Vector3Int(x, y, 0);
                var tile = tilemap.GetTile(cell);
                if (tile != grassTile && tile != lightGrassTile) continue;

                int cGrassCount = (int)(amountPerTile * (tile == lightGrassTile ? 0.3f : 1));

                // Spawn grass
                for (int i = 0; i < cGrassCount; i++)
                {
                    float r = Random.value * totalGrassWeight;
                    GrassEntry choice = grassEntries[0];
                    foreach (var e in grassEntries) { if (r < e.weight) { choice = e; break; } r -= e.weight; }

                    Vector3 world = tilemap.CellToWorld(cell);
                    Vector3 offset = new Vector3(
                        Random.Range(-maxOffset.x, maxOffset.x), 0,
                        Random.Range(-maxOffset.y, maxOffset.y) + 0.5f
                    );
                    Vector3 spawnPos = world + offset + tilemap.tileAnchor + new Vector3(0, choice.yOffset, 0);

                    var grass = CreateGrassObject(choice.prefab, spawnPos);
                    _spawnedObjects.Add(grass.transform);

                    round++;
                    if (round >= 3000) { round = 0; yield return null; }
                }

                if (tile != grassTile) continue;

                // Spawn flowers
                for (int j = 0; j < flowerAmount; j++)
                {
                    float sampleX = (x + Random.value) * flowerNoiseScale;
                    float sampleY = (y + Random.value) * flowerNoiseScale;
                    float noiseValue = Mathf.PerlinNoise(sampleX, sampleY);
                    if (noiseValue < flowerThreshold) continue;

                    float r2 = Random.value * totalFlowerWeight;
                    GrassEntry flower = flowerEntries[0];
                    foreach (var f in flowerEntries) { if (r2 < f.weight) { flower = f; break; } r2 -= f.weight; }

                    Vector3 worldF = tilemap.CellToWorld(cell);
                    Vector3 offsetF = new Vector3(
                        Random.Range(-maxOffset.x, maxOffset.x), 0,
                        Random.Range(-maxOffset.y, maxOffset.y) + 0.5f
                    );
                    Vector3 spawnPosF = worldF + offsetF + tilemap.tileAnchor + new Vector3(0, flower.yOffset, 0);

                    var flowerObj = CreateGrassObject(flower.prefab, spawnPosF);
                    _spawnedObjects.Add(flowerObj.transform);

                    round++;
                    if (round >= 3000) { round = 0; yield return null; }
                }
            }

        Debug.Log($"Grass and flowers spawned: {_spawnedObjects.Count} objects");
        _isSpawning = false;
    }

    private GameObject CreateGrassObject(GameObject prefab, Vector3 position)
    {
        GameObject grass = null;

#if UNITY_EDITOR
        grass = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
#else
            grass = Instantiate(prefab, parent);
#endif

        if (useNonSerializedObjects)
        {
            // Make object non-serialized so it won't save to scene
            grass.hideFlags = HideFlags.DontSave;
        }


        grass.transform.position = position;
        grass.transform.rotation = Quaternion.identity;

#if UNITY_EDITOR
        if (useNonSerializedObjects)
        {
            // Make object non-serialized so it won't save to scene
            grass.hideFlags = HideFlags.DontSave;
        }
#endif

        return grass;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(EditableGrassSpawner))]
    public class EditableGrassEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var spawner = (EditableGrassSpawner)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Grass Generation", EditorStyles.boldLabel);

            if (GUILayout.Button("Spawn Grass & Flowers"))
            {
                spawner.StartSpawnGrass();
                if (!spawner.useNonSerializedObjects)
                    EditorSceneManager.MarkSceneDirty(spawner.gameObject.scene);
            }

            if (GUILayout.Button("Clear All Grass"))
            {
                spawner.ClearAllGrass();
                if (!spawner.useNonSerializedObjects)
                    EditorSceneManager.MarkSceneDirty(spawner.gameObject.scene);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Data Management", EditorStyles.boldLabel);

            if (GUILayout.Button("Save Grass Data"))
            {
                spawner.SaveGrassData();
            }

            if (GUILayout.Button("Load Grass Data"))
            {
                spawner.LoadGrassData();
            }

            if (GUILayout.Button("Convert to Serialized Objects"))
            {
                ConvertToSerializedObjects(spawner);
            }

            EditorGUILayout.Space();

            if (spawner.useNonSerializedObjects)
            {
                EditorGUILayout.HelpBox("Non-serialized mode: Objects won't save to scene. Use 'Save Grass Data' to persist your changes.", MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("Serialized mode: Objects will save to scene file. May cause large file sizes with many objects.", MessageType.Warning);
            }
        }

        private void ConvertToSerializedObjects(EditableGrassSpawner spawner)
        {
            if (spawner.parent == null) return;

            foreach (Transform child in spawner.parent)
            {
                child.gameObject.hideFlags = HideFlags.None;
            }

            spawner.useNonSerializedObjects = false;
            EditorSceneManager.MarkSceneDirty(spawner.gameObject.scene);
            Debug.Log("Converted grass objects to serialized (will save to scene)");
        }
    }
#endif
}


