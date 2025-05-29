using System.Collections.Generic;
using DialogueGraph.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Stella : Interacteble
{
    public bool annoyed;
    public bool getHasDog()
    {
        return InventoryManager.Instance.hasItem("dog");
    }
    public bool hasBerry()
    {
        return InventoryManager.Instance.flags.Contains("berry");
    }

    public bool isAnnoyed()
    {
        return annoyed;
    }
    public void annoy()
    {
        annoyed = true;
    }

    public void GiveKey()
    {
        InventoryManager.Instance.flags.Add("stellakey");
    }

    public bool TalkingDog()
    {
        return InventoryManager.Instance.flags.Contains("talkingdog");
    }

    public bool StellaClose()
    {
        return InventoryManager.Instance.flags.Contains("stellaclose");
    }

    public void Distract()
    {
        InventoryManager.Instance.flags.Add("stelladistracted");
    }
}
