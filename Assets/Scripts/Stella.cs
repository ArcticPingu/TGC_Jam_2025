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

    public bool isAnnoyed()
    {
        return annoyed;
    }
    public void annoy()
    {
        annoyed = true;
    }
}
