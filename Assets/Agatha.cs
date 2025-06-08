using UnityEngine;

public class Agatha : Interacteble
{
    public bool HasWire()
    {
        return InventoryManager.Instance.hasItem("wire");
    }

    public void Generosity()
    {
        interactable = false;
        GameManager.Instance.Generosity();
    }

    public void ManuelSolve()
    {
        FindAnyObjectByType<GartenOuzle>().Clean();
        
        InventoryManager.Instance.flags.Add("mowerrepaired");
    }
}
