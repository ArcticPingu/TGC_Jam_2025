using System.Collections.Generic;
using UnityEngine;

public class Interacter : MonoBehaviour
{

    public List<Interacteble> localDistantSortedObjects = new();
    private float reachDistance = 2.5f;
    
    void Start()
    {
        InvokeRepeating(nameof(UpdateVisibility), 1, 0.1f);
    }
    void OnInteract()
    {
        Sort();

        if (localDistantSortedObjects.Count > 0 && Vector3.Distance(localDistantSortedObjects[0].transform.position, transform.position) < reachDistance)
        {
            localDistantSortedObjects[0].Interact();
        }

    }

    void UpdateVisibility()
    {
        Sort();

        for (int i = 0; i < localDistantSortedObjects.Count; i++)
        {
            if (Vector3.Distance(localDistantSortedObjects[i].transform.position, transform.position) < reachDistance)
            {
                localDistantSortedObjects[i].Hidden = false;
            }
            else
            {
                localDistantSortedObjects[i].Hidden = true;
            }
            
        }
    }

    void Sort()
    {
        localDistantSortedObjects.Sort((a, b) => (a.transform.position - transform.position).sqrMagnitude.CompareTo((b.transform.position - transform.position).sqrMagnitude));
    }

}
