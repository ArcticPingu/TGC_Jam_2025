using System.Collections.Generic;
using DialogueGraph.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class Stella : Interacteble
{
    public bool annoyed;
    public bool codeOnce;
    public bool needsCode;
    public static bool optmial;


    public bool getHasDog()
    {
        return InventoryManager.Instance.hasItem("dog", false);
    }
    public bool hasBerry()
    {
        return InventoryManager.Instance.hasItem("berry", false);
    }

    public void RemoveDog()
    {
        InventoryManager.Instance.hasItem("dog", true);
    }
    public void RemoveBerry()
    {
        InventoryManager.Instance.hasItem("berry", true);
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
        interactable = false;
        GameManager.Instance.Generosity();
    }

    public bool NeedsCode()
    {
        return needsCode;
    }

    public void AddCodeNeed()
    {
        needsCode = true;
    }

    public bool CodeTwice()
    {
        if (!codeOnce)
        {
            codeOnce = true;
            return false;
        }
        return true;
    }

    public bool IsOptimal()
    {
        return optmial;
    }

    public void UnOptimal()
    {
        optmial = false;
        Debug.LogWarning("NotOptimal");
    }

    public void End()
    {
        GameManager.endCounter += 1;
        interactable = false;

        if (GameManager.endCounter == 4)
        {
            GameManager.Instance.Credits();
        }
    }
}
