using UnityEngine;

public class PlayingCard : Interacteble
{
    public InventoryItem item;
    public bool hasCard;

    public void TakeCard()
    {
        InventoryManager.Instance.AddItem(item);
        GameManager.Instance.SpendPoint(1);
        hasCard = false;
    }

    public bool HasCard()
    {
        return hasCard;
    }
}
