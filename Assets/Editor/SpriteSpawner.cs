using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

[ExecuteInEditMode]
public class Tilemap3DSpriteSpawner : MonoBehaviour
{
    public Tilemap tilemap; // Assign your tilemap
    public GameObject[] spritePrefabs; // Assign 5 prefabs with SpriteRenderers
    public int objectsPerTile = 2;
    public Vector3 spawnOffset = new Vector3(0, 0.5f, 0); // Spread offset

    [ContextMenu("Spawn Sprites On Tilemap")]
    public void SpawnSprites()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap is not assigned.");
            return;
        }


        ClearChildren();

        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos))
                continue;

            Vector3 worldPos = tilemap.CellToWorld(pos) + tilemap.tileAnchor;

            for (int i = 0; i < objectsPerTile; i++)
            {
                GameObject prefab = spritePrefabs[Random.Range(0, spritePrefabs.Length)];
                Vector3 randomOffset = new Vector3(
                    Random.Range(-1f, 1f),
                    spawnOffset.y,
                    Random.Range(-1f, 1f)
                );

                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                instance.transform.position = worldPos + randomOffset;
                instance.transform.SetParent(this.transform);
            }
        }

        Debug.Log("Sprites spawned successfully.");
    }

    private void ClearChildren()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
