using System.Collections.Generic;
using UnityEngine;

public abstract class Interacteble : MonoBehaviour
{
    void Awake()
    {
        foreach (var item in FindObjectsByType<Interacter>(FindObjectsSortMode.None))
        {
            item.localDistantSortedObjects.Add(this);
        }

        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        foreach (var item in FindObjectsByType<Interacter>(FindObjectsSortMode.None))
        {
            item.localDistantSortedObjects.Remove(this);
        }
    }

    public abstract void Interact();
}
