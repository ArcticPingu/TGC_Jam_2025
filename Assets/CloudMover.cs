using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public Vector3 moveSpeed = new Vector3(2f, 0f, 7f);
    public float zResetThreshold = 100f;
    public float zResetTo = -100f;

    public float xResetThreshold = 100f;
    public float xResetTo = -100f;

    public float xMin = -50f;
    public float xMax = 50f;
    public float zMin = -100f;
    public float zMax = 100f;

    void Start()
    {
        foreach (Transform child in transform)
        {
            float randX = Random.Range(xMin, xMax);
            float randZ = Random.Range(zMin, zMax);
            Vector3 pos = new Vector3(randX, child.position.y, randZ);
            child.position = pos;
        }
    }

    void Update()
    {
        foreach (Transform child in transform)
        {
            child.position += moveSpeed * Time.deltaTime;

            if (child.position.z > zResetThreshold)
            {
                Vector3 pos = child.position;
                pos.z = zResetTo;
                child.position = pos;
            }

            if (child.position.x > xResetThreshold)
            {
                Vector3 pos = child.position;
                pos.x = xResetTo;
                child.position = pos;
            }
        }
    }
}
