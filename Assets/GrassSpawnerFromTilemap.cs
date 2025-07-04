using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class GrassTilemapSpawner : MonoBehaviour
{
    [Tooltip("The Tilemap to scan")]
    public Tilemap tilemap;

    [Tooltip("Grass prefabs to spawn (at least 4)")]
    public GameObject[] grassPrefabs;

    [Tooltip("Max random offset from tile center")]
    public Vector2 maxOffset = new Vector2(0.3f, 0.3f);

    [Tooltip("Parent object for spawned grass")]
    public Transform parent;

    // Call this from Editor menu or inspector button
    public void SpawnGrass()
    {

        if (parent == null)
            parent = this.transform;

        // Clear previous grass children if any
        int childCount = parent.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(parent.GetChild(i).gameObject);
        }

        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(cellPos);
                if (tile != null)
                {
                    // Pick random prefab
                    GameObject grassPrefab = grassPrefabs[Random.Range(0, grassPrefabs.Length)];

                    // Convert cell to world position
                    Vector3 worldPos = tilemap.CellToWorld(cellPos);

                    // Add random offset (X and Z for 3D, or X and Y for 2D)
                    Vector3 offset = new Vector3(
                        Random.Range(-maxOffset.x, maxOffset.x),
                        0,
                        Random.Range(-maxOffset.y, maxOffset.y)
                    );

                    Vector3 spawnPos = worldPos + offset;

                    GameObject grass = (GameObject)PrefabUtility.InstantiatePrefab(grassPrefab, parent);
                    grass.transform.position = spawnPos;
                    grass.transform.rotation = Quaternion.identity;
                    grass.transform.localScale = Vector3.one;
                }
            }
        }

        Debug.Log("Grass spawned on Tilemap.");
    }
}
