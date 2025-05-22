using System.Collections.Generic;
using UnityEngine;

public class Interacter : MonoBehaviour
{

    public List<Interacteble> localDistantSortedObjects = new();
    
    void Start()
    {
        InvokeRepeating(nameof(UpdateVisibility), 1, 0.1f);
    }
    void OnInteract()
    {
        Sort();

        if (localDistantSortedObjects.Count > 0 && Vector3.Distance(localDistantSortedObjects[0].transform.position, transform.position) < 3)
        {
            localDistantSortedObjects[0].Interact();
        }

    }

    void UpdateVisibility()
    {
        Sort();

        for (int i = 0; i < localDistantSortedObjects.Count; i++)
        {
            if (Vector3.Distance(localDistantSortedObjects[i].transform.position, transform.position) < 3)
            {
                localDistantSortedObjects[i].gameObject.SetActive(true);
            }
            else
            {
                localDistantSortedObjects[i].gameObject.SetActive(false);
            }
            
        }
    }

    void Sort()
    {
        localDistantSortedObjects.Sort((a, b) => (a.transform.position - transform.position).sqrMagnitude.CompareTo((b.transform.position - transform.position).sqrMagnitude));
    }

}
