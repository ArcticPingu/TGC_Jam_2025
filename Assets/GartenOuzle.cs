using UnityEngine;
using UnityEngine.Tilemaps;

public class GartenOuzle : MonoBehaviour
{
    public PlayerController player;
    private Vector3[][] positions = new Vector3[4][];
    private GameObject[][] grassObjects = new GameObject[4][];
    public Tilemap tilemap;
    public Tile grass;
    public GameObject grassObject;

    void Awake()
    {
        player = FindAnyObjectByType<PlayerController>();

        // Initialize each inner array
        for (int i = 0; i < 4; i++)
        {
            positions[i] = new Vector3[4];
            grassObjects[i] = new GameObject[4];
        }

        // Populate the positions
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                positions[i][j] = transform.position + new Vector3(i, 0, j - 0.25f);
                Instantiate(grassObject, new Vector3(positions[i][j].x, 0, positions[i][j].z), Quaternion.identity);
            }
        }

        // Set tiles
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                tilemap.SetTile(
                    new Vector3Int(Mathf.CeilToInt(positions[i][j].x) - 1, Mathf.CeilToInt(positions[i][j].z) - 1, 0),
                    grass
                );
            }
        }
    }


    public void StartPuzzle()
    {
        // player
    }
}
