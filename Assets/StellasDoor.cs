using TMPro;
using UnityEngine;

public class StellasDoor : Interacteble
{
    public ushort correct;
    public InventoryItem item;
    public bool once = true;
    public bool wasToldCode()
    {
        return InventoryManager.Instance.flags.Contains("stellakey");
    }

    public void Correct()
    {
        correct++;
    }

    public void Reset()
    {
        correct = 0;
    }

    public bool isCorrect()
    {
        return correct == 4;
    }

    public void StellaClose()
    {
        InventoryManager.Instance.flags.Add("stellaclose");
    }
    public bool Distracted()
    {
        return InventoryManager.Instance.flags.Contains("stelladistracted");
    }
    public void Bone()
    {
        InventoryManager.Instance.AddItem(item);
    }

    public bool GetOnce()
    {
        if (once)
        {
            once = false;
            return true;
        }
        return false;
    }
}
