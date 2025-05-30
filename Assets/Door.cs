using Unity.VisualScripting;
using UnityEngine;

public class Door : Interacteble
{
    public bool closed;
    public void Close()
    {
        closed = true;
        FindAnyObjectByType<GateObject>().Close();
        InventoryManager.Instance.flags.Add("closeddoor");
    }

    public bool isClosed()
    {
        InventoryManager.Instance.flags.Add("sawdoor");
        return closed;
    }

    public bool hasScrewdriver()
    {
        return InventoryManager.Instance.hasItem("screwdriver");
    }

    public bool OtherSide()
    {
        return FindAnyObjectByType<PlayerController>().gameObject.transform.position.z > transform.position.z;
    }

    public void EndDay()
    {
        FindAnyObjectByType<EndDay>().End();
    }
}
