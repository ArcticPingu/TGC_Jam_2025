using Unity.VisualScripting;
using UnityEngine;

public class Dog : Interacteble
{
    public InventoryItem item;
    public GameObject dog;
    public bool Puzzle()
    {
        return InventoryManager.Instance.hasItem("berry", false);
    }

    public void TalkingDog()
    {
        InventoryManager.Instance.flags.Add("talkingdog");
    }

    public void AddDog()
    {
        InventoryManager.Instance.AddItem(item);
        dog.SetActive(false);
        interactable = false;
    }

    public bool hasBone()
    {
        return InventoryManager.Instance.hasItem("bone", false);
    }
    
    public void RemoveBone()
    {
        InventoryManager.Instance.hasItem("bone", true);
    }


    public void AcceptedRidle()
    {
        Stella.optmial = false;
        Debug.LogWarning("NotOptimal");
    }
}
