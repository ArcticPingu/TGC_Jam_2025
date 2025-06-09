using System.Collections.Generic;
using UnityEngine;

public class Interacter : MonoBehaviour
{

    public List<Interacteble> localDistantSortedObjects = new();
    public float reachDistance = 2.5f;
    public Interacteble curentinteracteble;

    public static bool doneInteract;


    void Start()
    {
        InvokeRepeating(nameof(UpdateVisibility), 1, 0.1f);
    }
    void OnInteract()
    {
        Sort();

        if (localDistantSortedObjects.Count > 0 && Vector3.Distance(localDistantSortedObjects[0].transform.position, transform.position) < reachDistance)
        {
            if (curentinteracteble == null)
            {
                curentinteracteble = localDistantSortedObjects[0];
                curentinteracteble.Interact(GetComponent<PlayerController>());
            }
        }
    }

    void OnContinue()
    {
        if (curentinteracteble != null)
            curentinteracteble.Continue();
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

    public void ForceInteract(Interacteble interacteble)
    {
        curentinteracteble = interacteble;
        curentinteracteble.Interact(GetComponent<PlayerController>());
    }

}
