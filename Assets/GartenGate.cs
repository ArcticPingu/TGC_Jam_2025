using UnityEngine;

public class GartenGate : Interacteble
{
    void OnCollisionEnter(Collision collision)
    {
        Interact(collision.collider.GetComponent<PlayerController>());
    }

    public void StartPuzle()
    {
        
    }

    public bool HasMower()
    {
        return InventoryManager.Instance.hasItem("mower");
    }
}
