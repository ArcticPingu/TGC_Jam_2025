using UnityEngine;

public class GartenGate : Interacteble
{
    public GartenOuzle puzzle;
    bool interacteble = true;
    void OnCollisionEnter(Collision collision)
    {
        if (!interacteble)
            return;

        Debug.Log("garten Enter");
        collision.collider.GetComponentInParent<Interacter>().ForceInteract(this);
    }

    public void StartPuzle()
    {
        interactable = true;
        puzzle.StartPuzzle();
    }

    public bool HasMower()
    {
        return InventoryManager.Instance.hasItem("mower");
    }
}
