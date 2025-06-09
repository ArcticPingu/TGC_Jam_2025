using Unity.VisualScripting;
using UnityEngine;

public class BerryBush : Interacteble
{

    public int id;
    public InventoryItem item;
    public static int index = 0;
    public int[] order = { 1, 3, 7 };
    public bool hasBerry = true;

    public bool inOrder()
    {
        int tmp = index;
        index++;
        return order[tmp] == id;
    }

    public void Reset()
    {
        index = 0;
    }

    public bool HasBerry()
    {
        return hasBerry;
    }

    public void AddBerry()
    {
        hasBerry = false;
        InventoryManager.Instance.AddItem(item);
    }

    public bool allDone()
    {
        return id == 7 && index == 2;
    }
}
