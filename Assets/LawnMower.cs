using UnityEngine;

public class LawnMower : Interacteble
{
    public InventoryItem item;
    public GameObject worldObject;
    public bool HasWire()
    {
        return InventoryManager.Instance.hasItem("wire", false);
    }

    public void RemoveWire()
    {
        InventoryManager.Instance.hasItem("wire", true);
    }
    

    public void PickUp()
    {
        InventoryManager.Instance.AddItem(item);
        SetInteracteble(false);
        worldObject.SetActive(false);
    }
}
