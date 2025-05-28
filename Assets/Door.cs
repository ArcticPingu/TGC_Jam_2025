using UnityEngine;

public class Door : Interacteble
{
    public bool closed;
    public override void Interact()
    {
        if (isInConversation)
            return;

        DialogueSystem.ResetConversation();
        isInConversation = true;
        (showPlayer ? PlayerContainer : NpcContainer).SetActive(true);
    }

    public void Close()
    {
        closed = true;
    }

    public bool isClosed()
    {
        return closed;
    }

    public bool hasScrewdriver()
    {
        return InventoryManager.Instance.hasItem("screwdriver");
    }
}
