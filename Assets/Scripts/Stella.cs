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
        return InventoryManager.Instance.hasItem("berry");
    }
    public bool doorClosed()
    {
        return InventoryManager.Instance.flags.Contains("closeddoor");
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
    public bool SawGate()
    {
        Debug.Log(InventoryManager.Instance.flags.Contains("sawdoor"));
        return InventoryManager.Instance.flags.Contains("sawdoor");
    }
    public bool RepairedGate(string id)
    {
        return InventoryManager.Instance.flags.Contains("closeddoor");
    }

    public void Generosity()
    {
        GameManager.Instance.Generosity();
    }
}
