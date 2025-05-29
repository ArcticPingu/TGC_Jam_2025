using System.Collections.Generic;
using UnityEngine;

public class Interacter : MonoBehaviour
{

    public List<Interacteble> localDistantSortedObjects = new();
    public float reachDistance = 2.5f;
    
    void Start()
    {
        InvokeRepeating(nameof(UpdateVisibility), 1, 0.1f);
    }
    void OnInteract()
    {
        Sort();

        if (localDistantSortedObjects.Count > 0 && Vector3.Distance(localDistantSortedObjects[0].transform.position, transform.position) < reachDistance)
        {
            localDistantSortedObjects[0].Interact(GetComponent<PlayerController>());
        }

    }

    void UpdateVisibility()
    {
        Sort();

        if (Vector3.Distance(localDistantSortedObjects[0].transform.position, transform.position) < reachDistance)
        {
            localDistantSortedObjects[0].Hidden = false;
        }
        else
        {
            localDistantSortedObjects[0].Hidden = true;
        }

        for (int i = 1; i < localDistantSortedObjects.Count; i++)
        {
            localDistantSortedObjects[i].Hidden = true;
        }
    }

    void Sort()
    {
        localDistantSortedObjects.Sort((a, b) => (a.transform.position - transform.position).sqrMagnitude.CompareTo((b.transform.position - transform.position).sqrMagnitude));
    }

}
