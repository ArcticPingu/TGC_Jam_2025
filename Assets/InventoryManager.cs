using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<string> inventory = new();

    public bool hasItem(string item)
    {
        return inventory.Contains(item);
    }


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);    
        }

        Instance = this;
    }
}
