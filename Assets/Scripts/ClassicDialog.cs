using System.Collections.Generic;
using DialogueGraph.Runtime;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class ClassicDialog : Interacteble
{
    public override void Interact()
    {
        if (isInConversation)
            return;

        DialogueSystem.ResetConversation();
        isInConversation = true;
        (showPlayer ? PlayerContainer : NpcContainer).SetActive(true);
    }

    public RuntimeDialogueGraph DialogueSystem;

    [Header("UI References")]
    public GameObject PlayerContainer;
    public GameObject NpcContainer;
    public Transform ButtonParent;
    public TMP_Text PlayerText;
    public TMP_Text NpcText;
    public TMP_Text NpcName;

    private bool isInConversation = false;
    private bool showPlayer;
    private bool isPlayerChoosing;
    private bool shouldShowText;
    private bool showingText;
    private string textToShow;

    private void Update()
    {

        if (!isInConversation || isPlayerChoosing) return;

        if (shouldShowText)
        {
            (showPlayer ? PlayerContainer : NpcContainer).SetActive(true);
            (showPlayer ? PlayerText : NpcText).gameObject.SetActive(true);

            (showPlayer ? PlayerText : NpcText).text = textToShow;
            (showPlayer ? PlayerText : NpcText).GetComponent<TextAnimator>().ForceParseLocal();

            showingText = true;
            shouldShowText = false;
        }

        if (showingText)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (TextAnimator.animating)
                {
                    TextAnimator.animator.Finish();
                    return;
                }

                showingText = false;
                (showPlayer ? PlayerContainer : NpcContainer).SetActive(false);
                (showPlayer ? PlayerText : NpcText).gameObject.SetActive(false);
            }
        }
        else
        {
            if (DialogueSystem.IsConversationDone())
            {
                // Reset state
                isInConversation = false;
                showPlayer = false;
                isPlayerChoosing = false;
                shouldShowText = false;
                showingText = false;

                PlayerContainer.SetActive(false);
                NpcContainer.SetActive(false);
                return;
            }

            bool isNpc = DialogueSystem.IsCurrentNpc();
            if (isNpc)
            {
                var currentActor = DialogueSystem.GetCurrentActor();
                showPlayer = false;
                shouldShowText = true;
                Debug.Log(currentActor);
                NpcName.text = currentActor.Name;
                textToShow = DialogueSystem.ProgressNpc();
            }
            else
            {
                isPlayerChoosing = true;
                PlayerContainer.SetActive(true);

                List<ConversationLine> lines = DialogueSystem.GetCurrentLines();

                for (int i = 0; i < 3; i++)
                {
                    ButtonParent.GetChild(i).gameObject.SetActive(i < lines.Count);

                    if (i < lines.Count)
                    {
                        if (!DialogueSystem.ExecuteChecks(lines[i], i))
                        {
                            ButtonParent.GetChild(i).gameObject.SetActive(false);
                            continue;
                        }    

                        ButtonParent.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = lines[i].Message;
                        ButtonParent.GetChild(i).GetComponentInChildren<TextAnimator>().ForceParseLocal();
                        Button button = ButtonParent.GetChild(i).GetComponentInChildren<Button>();
                        button.onClick.RemoveAllListeners(); // Remove existing listeners
                        int index = i;
                        button.onClick.AddListener(() => Click(index));
                        Debug.Log(i);
                    }
                }

            }
        }
    }

    public void Click(int index)
    {
        Debug.Log(index);

        for (int i = 0; i < 3; i++)
        {
            ButtonParent.GetChild(i).gameObject.SetActive(false);
        }

        textToShow = DialogueSystem.ProgressSelf(index);
        isPlayerChoosing = false;
        shouldShowText = true;
        showPlayer = true;
    }

    public void ActionPoint()
    {
        GameManager.Instance.currentActionPoints--;
    }

    public bool meetBefore;

    public void setMeetBeforTrue()
    {
        meetBefore = true;
    }

    public bool getMeetBefore()
    {
        return meetBefore;
    }

    public bool hasDog;
    
    public void setHasDogTrue()
    {
        hasDog = true;
    }

    public bool getHasDog()
    {
        return hasDog;
    }
}
