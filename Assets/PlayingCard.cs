using UnityEngine;

public class PlayingCard : Interacteble
{
    public InventoryItem item;
    public bool hasCard;
    public Sprite sprite;
    public SpriteRenderer bush;

    public void TakeCard()
    {
        bush.sprite = sprite;
        InventoryManager.Instance.AddItem(item);
        GameManager.Instance.SpendPoint(1);
        hasCard = false;
    }

    public bool HasCard()
    {
        return hasCard;
    }
}
