using Unity.VisualScripting;
using UnityEngine;

public class BerryBush : Interacteble
{

    public int id;
    public InventoryItem item;
    public static int index = 0;
    public int[] order = { 1, 3, 7 };

    public bool inOrder()
    {
        return order[index++] == id;
    }

    public void Reset()
    {
        InventoryManager.Instance.flags.Remove("bush3");
        InventoryManager.Instance.flags.Remove("bush7");
    }

    public void AddBerry()
    {
        InventoryManager.Instance.AddItem(item);
    }

    public bool allDone()
    {
        if (id == 7 && order[index] == id)
        {
            InventoryManager.Instance.flags.Add("puzle1");
        }

        return id == 7 && order[index] == id;
    }
}
