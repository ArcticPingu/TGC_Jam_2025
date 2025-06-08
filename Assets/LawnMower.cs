using UnityEngine;

public class LawnMower : Interacteble
{
    public InventoryItem item;
    public GameObject worldObject;
    public bool HasWire()
    {
        return InventoryManager.Instance.hasItem("wire");
    }

    public void PickUp()
    {
        InventoryManager.Instance.AddItem(item);
        interactable = false;
        worldObject.SetActive(false);
    }
}
