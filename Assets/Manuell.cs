using UnityEngine;

public class Manuell : Interacteble
{
    public ushort correct;
    public bool HasCard()
    {
        return InventoryManager.Instance.hasItem("card");
    }

    public void Generosity()
    {
        GameManager.Instance.Generosity();
    }

    public void Correct()
    {
        correct++;
    }

    public bool isCorrect()
    {
        return correct == 4;
    }

        public void Reset()
    {
        correct = 0;
    }
}
