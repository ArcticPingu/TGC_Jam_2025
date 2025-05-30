using UnityEngine;

public class Toolbox : Interacteble
{
    public bool once = true;
    public InventoryItem item;


    public bool GetOnce()
    {
        if (once)
        {
            once = false;
            return true;
        }
        return false;
    }

    public void TakeScrewdriver()
    {
        InventoryManager.Instance.AddItem(item);
    }
}
