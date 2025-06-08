using UnityEngine;

public class PlayingCard : Interacteble
{
    public InventoryItem item;
    public void TakeCard()
    {
        InventoryManager.Instance.AddItem(item);
    }
}
