using Unity.VisualScripting;
using UnityEngine;

public class Door : Interacteble
{
    public bool closed;
    public void Close()
    {
        closed = true;
        FindAnyObjectByType<GateObject>().Close();
    }

    public bool isClosed()
    {
        return closed;
    }

    public bool hasScrewdriver()
    {
        return InventoryManager.Instance.hasItem("screwdriver");
    }
}
