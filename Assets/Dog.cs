using UnityEngine;

public class Dog : Interacteble
{
    public InventoryItem item;
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
    }

    public bool hasBone()
    {
        return InventoryManager.Instance.hasItem("bone");
    }
}
