using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class GrassTilemapSpawner : MonoBehaviour
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

    [Tooltip("Max random X/Z offset from tile center")]
    public Vector2 maxOffset = new Vector2(0.3f, 0.3f);

    [Tooltip("Number of grass instances to spawn per tile")]
    [Min(1)]
    public int amountPerTile = 1;

    [Tooltip("Parent object for spawned grass")]
    public Transform parent;

    public void SpawnGrass()
    {
        if (parent == null)
            parent = transform;

        while (parent.childCount > 0)
            DestroyImmediate(parent.GetChild(0).gameObject);

        float totalWeight = 0;
        foreach (var e in grassEntries)
            totalWeight += e.weight;

        var bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                var cell = new Vector3Int(x, y, 0);
                if (tilemap.GetTile(cell) == null) continue;

                for (int i = 0; i < amountPerTile; i++)
                {
                    // Weighted random selection
                    float r = Random.value * totalWeight;
                    GrassEntry choice = grassEntries[0];
                    foreach (var e in grassEntries)
                    {
                        if (r < e.weight) { choice = e; break; }
                        r -= e.weight;
                    }

                    Vector3 world = tilemap.CellToWorld(cell);
                    Vector3 offset = new Vector3(
                        Random.Range(-maxOffset.x, maxOffset.x),
                        0,
                        Random.Range(-maxOffset.y, maxOffset.y)
                    );

                    Vector3 spawnPos = world + offset + tilemap.tileAnchor + new Vector3(0, choice.yOffset, 0);

                    GameObject grass = Instantiate(choice.prefab, parent);
                    grass.transform.position = spawnPos;
                    grass.transform.rotation = Quaternion.identity;
                }
            }

        Debug.Log("Grass (re)spawned with weights, offsets, and amount per tile.");
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GrassTilemapSpawner))]
    public class GTS_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Spawn Grass"))
                (target as GrassTilemapSpawner).SpawnGrass();
        }
    }
#endif
}
