using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public InventoryItem[] inventory = new InventoryItem[5];
    public Transform[] inventoryHolder;
    public List<string> flags = new();
    public int n;

    public bool hasItem(string item)
    {
        bool hasItem = false;

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null && inventory[i].id == item)
            {
                hasItem = true;
            }
        }

        return hasItem;
    }

    void Update()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null)
            {
                inventoryHolder[i].GetComponent<Image>().sprite = inventory[i].sprite;
                inventoryHolder[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                inventoryHolder[i].GetComponent<Image>().sprite = null;
                inventoryHolder[i].GetComponent<Image>().color = new Color(0, 0, 0, 0);
            }

        }
    }


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;

        InventoryManager.Instance.flags.Add("bush1");
    }

    public void AddItem(InventoryItem item)
    {
        inventory[n++] = item;
    }
}
