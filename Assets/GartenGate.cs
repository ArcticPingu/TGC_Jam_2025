using UnityEngine;

public class GartenGate : Interacteble
{
    public GartenOuzle puzzle;
    bool interactebleCollsion = true;

    void OnCollisionEnter(Collision collision)
    {
        if (!interactebleCollsion)
            return;

        Debug.Log("garten Enter");
        collision.collider.GetComponentInParent<Interacter>().ForceInteract(this);
    }

    public void StartPuzle()
    {
        interactebleCollsion = true;
        puzzle.StartPuzzle();
    }

    public bool HasMower()
    {
        return InventoryManager.Instance.hasItem("mower");
    }
}
