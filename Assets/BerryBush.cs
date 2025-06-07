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
        int tmp = index;
        index++;
        Debug.Log(tmp);
        return order[tmp] == id || order[Mathf.Abs(2 - tmp)] == id;
    }

    public void Reset()
    {
        index = 0;
    }

    public void AddBerry()
    {
        InventoryManager.Instance.AddItem(item);
    }

    public bool allDone()
    {
        return id == 7 && index == 2;
    }

    public bool allDoneReverse()
    {
        return id == 1 && index == 2;
    }
}
