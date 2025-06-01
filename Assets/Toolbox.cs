using UnityEngine;

public class Toolbox : Interacteble
{
    public bool screwdriverInBox = true;
    public InventoryItem item;


    public bool GetScrewdriverInBox()
    {
        return screwdriverInBox;
    }

    public void TakeScrewdriver()
    {
        InventoryManager.Instance.AddItem(item);
        screwdriverInBox = false;
    }
}
