using UnityEngine;

public class Toolbox : Interacteble
{
    public bool screwdriverInBox = true;
    public bool wireInBox = true;
    public InventoryItem screwdriver;
    public InventoryItem wire;


    public bool GetScrewdriverInBox()
    {
        return screwdriverInBox;
    }
    public bool GetWireInBox()
    {
        return wireInBox;
    }

    public void TakeScrewdriver()
    {
        InventoryManager.Instance.AddItem(screwdriver);
        screwdriverInBox = false;
    }
    public void TakeWire()
    {
        InventoryManager.Instance.AddItem(wire);
        wireInBox = false;
    }
}
