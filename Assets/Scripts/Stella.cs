using System.Collections.Generic;
using DialogueGraph.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Stella : Interacteble
{
    public bool annoyed;
    public override void Interact()
    {
        if (isInConversation)
            return;

        DialogueSystem.ResetConversation();
        isInConversation = true;
        (showPlayer ? PlayerContainer : NpcContainer).SetActive(true);
    }
    public bool getHasDog()
    {
        return InventoryManager.Instance.hasItem("dog");
    }

    public bool isAnnoyed()
    {
        return annoyed;
    }
    public void annoy()
    {
        annoyed = true;
    }
}
