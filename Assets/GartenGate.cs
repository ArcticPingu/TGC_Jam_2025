using UnityEngine;

public class GartenGate : Interacteble
{
    public GartenOuzle puzzle;
    public bool interactebleCollsion = true;

    void OnCollisionEnter(Collision collision)
    {
        if (!interactebleCollsion)
            return;

        Debug.Log("garten Enter");
        collision.collider.GetComponentInParent<Interacter>().ForceInteract(this);
    }

    public void StartPuzle()
    {
        interactebleCollsion = false;
        puzzle.StartPuzzle();
    }

    public bool HasMower()
    {
        return InventoryManager.Instance.hasItem("mower", false);
    }
}
