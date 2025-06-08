using UnityEngine;

public class Agatha : Interacteble
{
    public bool HasWire()
    {
        return InventoryManager.Instance.hasItem("wire");
    }
}
