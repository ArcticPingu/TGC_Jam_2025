using UnityEngine;
using System.Collections.Generic;

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

    public float minSpacing = 10f;

    void Start()
    {
        SpreadCloudsEvenly();
    }

    void SpreadCloudsEvenly()
    {
        List<Vector3> usedPositions = new List<Vector3>();
        int maxAttempts = 100;

        foreach (Transform child in transform)
        {
            bool placed = false;
            int attempts = 0;

            while (!placed && attempts < maxAttempts)
            {
                float x = Random.Range(xMin, xMax);
                float z = Random.Range(zMin, zMax);
                Vector3 candidate = new Vector3(x, child.position.y, z);

                bool tooClose = false;
                foreach (var pos in usedPositions)
                {
                    if (Vector3.Distance(candidate, pos) < minSpacing)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    child.position = candidate;
                    usedPositions.Add(candidate);
                    placed = true;
                }

                attempts++;
            }

            if (!placed)
            {
                float x = Random.Range(xMin, xMax);
                float z = Random.Range(zMin, zMax);
                child.position = new Vector3(x, child.position.y, z);
            }
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
