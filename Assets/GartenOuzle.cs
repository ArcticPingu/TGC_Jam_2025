using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GartenOuzle : MonoBehaviour
{
    public PlayerController player;
    private Vector3[][] positions = new Vector3[4][];
    private GameObject[][] grassObjects = new GameObject[4][];
    public Tilemap tilemap;
    public Tile dirt;
    public GameObject grassObject;
    public GartenGate gartenGate;

    public bool startedGame;
    public CinemachineFollow cinemachine;

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
                
                if ((i != 0 || j != 0) && !(i == 0 && j == 3) && !(i == 2 && j == 0))
                    grassObjects[i][j] = Instantiate(grassObject, new Vector3(positions[i][j].x, 0.2f, positions[i][j].z), Quaternion.identity);
            }
        }
    }


    public void StartPuzzle()
    {
        won = true;
        startedGame = true;
        player.mowing = true;
        player.GetComponent<Rigidbody>().position = new Vector3(52.602f, 0.6631742f, -24.69f);

        cinemachine.FollowOffset = new Vector3(0, 8, -6);
    }

    public void EndGame()
    {
        startedGame = false;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (grassObjects[i][j] != null)
                {
                    if (!(i == 3 && j == 0))
                    {
                        won = false;
                        Debug.Log("MISSIED GRASS: " + i + " " + j);
                    }

                }
            }
        }

        if (won)
        {
            InventoryManager.Instance.flags.Add("grassCut");
        }
        else
        {
            InventoryManager.Instance.flags.Add("grassRuined");
        }

        player.GetComponent<Rigidbody>().position = new Vector3(51.079f, 0.6631742f, -24.69f);
        player.mowing = false;
        cinemachine.FollowOffset = new Vector3(0, 5.5f, -11);
        gartenGate.interactebleCollsion = true;
    }

    public void Clean()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (grassObjects[i][j] != null)
                {
                    Destroy(grassObjects[i][j]);
                }
            }
        }
    }

    int lastI;
    int lastJ;
    public bool won;

    void Update()
    {
        if (!startedGame)
            return;

        Vector3 pos = player.transform.position;

        int i = Mathf.RoundToInt(player.transform.position.x - transform.position.x);
        int j = Mathf.RoundToInt(player.transform.position.z - transform.position.z);

        Debug.Log("i: " + i);
        Debug.Log("j: " + j);

        if (i != 0 || j != 0)
        {
            if (i < 4 && i >= 0)
            {
                if (i != lastI || j != lastJ)
                {
                    UpdateTile(i, j);
                }

                if (i == 3 && j == 0)
                {
                    EndGame();
                }
            }
        }

        lastI = i;
        lastJ = j;
    }

    void UpdateTile(int i, int j)
    {

        if (grassObjects[i][j] != null)
        {
            Destroy(grassObjects[i][j]);
        }
        else
        {
            tilemap.SetTile(
                new Vector3Int(Mathf.CeilToInt(positions[i][j].x) - 1, Mathf.CeilToInt(positions[i][j].z) - 1, 0),
                dirt
            );

            Debug.Log("DOUBLED GRASS");
            won = false;
        }
    }
}
