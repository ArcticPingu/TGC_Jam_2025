using Unity.VisualScripting;
using UnityEngine;

public class Dog : Interacteble
{
    public InventoryItem item;
    public GameObject dog;
    public bool Puzzle()
    {
        return InventoryManager.Instance.flags.Contains("puzle1");
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
        return InventoryManager.Instance.hasItem("bone");
    }

    public void AcceptedRidle()
    {
        Stella.optmial = false;
         Debug.LogWarning("NotOptimal");
    }
}
