using UnityEngine;

public class Manuell : Interacteble
{
    public ushort correct;
    public bool HasCard()
    {
        return InventoryManager.Instance.hasItem("card", false);
    }

    public void RemoveCard()
    {
        InventoryManager.Instance.hasItem("card", true);
    }

    public void Generosity()
    {
        interactable = false;
        GameManager.Instance.Generosity();
    }

    public void Correct()
    {
        correct++;
    }

    public bool isCorrect()
    {
        return correct == 5;
    }

        public void Reset()
    {
        correct = 0;
    }
}
