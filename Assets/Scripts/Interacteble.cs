using System.Collections.Generic;
using DialogueGraph.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class Interacteble : MonoBehaviour
{
    public RuntimeDialogueGraph DialogueSystem;

    protected GameObject PlayerContainer;
    protected GameObject NpcContainer;
    protected Transform ButtonParent;
    protected TMP_Text PlayerText;
    protected TMP_Text NpcText;
    protected TMP_Text NpcName;
    protected Image npcImage;

    public PlayerController talker;

    protected bool isInConversation = false;
    protected bool showPlayer;
    protected bool isPlayerChoosing;
    protected bool shouldShowText;
    protected bool showingText;
    protected string textToShow;

    public string CostId;
    public bool interactable;

    protected
    void Awake()
    {
        foreach (var item in FindObjectsByType<Interacter>(FindObjectsSortMode.None))
        {
            item.localDistantSortedObjects.Add(this);
        }

        interactable = true;
    }

    void Start()
    {
        PlayerContainer = DialogRefHolder.Instance.PlayerContainer;
        NpcContainer = DialogRefHolder.Instance.NpcContainer;
        ButtonParent = DialogRefHolder.Instance.ButtonParent;
        PlayerText = DialogRefHolder.Instance.PlayerText;
        NpcText = DialogRefHolder.Instance.NpcText;
        NpcName = DialogRefHolder.Instance.NpcName;
        npcImage = DialogRefHolder.Instance.npcImage;
    }

    void OnDestroy()
    {
        foreach (var item in FindObjectsByType<Interacter>(FindObjectsSortMode.None))
        {
            item.localDistantSortedObjects.Remove(this);
        }
    }

    public void Interact(PlayerController player)
    {
        if (isInConversation || !interactable)
            return;

        DialogueSystem.ResetConversation();
        isInConversation = true;

        (showPlayer ? PlayerContainer : NpcContainer).SetActive(true);

        talker = player;

        talker.canMove = false;
    }


    private bool _hidden;
    public bool Hidden
    {
        get => _hidden;
        set
        {
            if (_hidden != value)
            {
                _hidden = value;
                if (!_hidden)
                {
                    Show();
                }
                else
                {
                    Hide();
                }
            }
        }
    }

    public float animationDuration = 0.1f;
    public void Show()
    {
        if (!interactable)
            return;

        // Pop in with a bounce
        LeanTween.scale(gameObject, Vector3.one, animationDuration)
            .setEase(LeanTweenType.easeOutBack);
    }

    public void Hide()
    {
        // Smoothly shrink out
        LeanTween.scale(gameObject, Vector3.zero, animationDuration)
            .setEase(LeanTweenType.easeInBack);
    }

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

        if (!showingText)
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

                Debug.Log("END");
                talker.canMove = true;
                talker.gameObject.GetComponent<Interacter>().curentinteracteble = null;
                talker = null;

                return;
            }

            bool isNpc = DialogueSystem.IsCurrentNpc();
            if (isNpc)
            {
                var currentActor = DialogueSystem.GetCurrentActor();
                Actor container = (Actor)currentActor.CustomData;
                npcImage.sprite = container.sprite;
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

                ushort count = 0;
                int lastIndex = 0;

                for (int i = 0; i < 6; i++)
                {
                    ButtonParent.GetChild(i).gameObject.SetActive(i < lines.Count);

                    if (i < lines.Count)
                    {
                        if (!DialogueSystem.ExecuteChecks(lines[i], i))
                        {
                            ButtonParent.GetChild(i).gameObject.SetActive(false);
                            continue;
                        }

                        count++;

                        ButtonParent.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = lines[i].Message;
                        ButtonParent.GetChild(i).GetComponentInChildren<TextAnimator>().ForceParseLocal();
                        ButtonParent.GetChild(i).GetComponent<FutureNeedle>().showFuture = false;

                        foreach (string item in lines[i].Triggers)
                        {
                            if (CostId == item)
                            {
                                ButtonParent.GetChild(i).GetComponent<FutureNeedle>().showFuture = true;
                            }

                            Debug.Log(item);
                        }

                        // 
                        Button button = ButtonParent.GetChild(i).GetComponentInChildren<Button>();
                        button.onClick.RemoveAllListeners(); // Remove existing listeners
                        int index = i;
                        button.onClick.AddListener(() => Click(index));
                        Debug.Log(i);

                        lastIndex = i;
                    }
                }

                if (count == 1)
                {
                    Click(lastIndex);
                }
                else
                {
                    
                    if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
                    {
                        EventSystem.current.SetSelectedGameObject(ButtonParent.GetChild(0).gameObject);
                    }
                }

            }
        }
    }

    public void Click(int index)
    {
        for (int i = 0; i < 6; i++)
        {
            ButtonParent.GetChild(i).gameObject.SetActive(false);
        }

        textToShow = DialogueSystem.ProgressSelf(index);
        isPlayerChoosing = false;
        shouldShowText = true;
        showPlayer = true;
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
    public void ActionPoint()
    {
        GameManager.Instance.SpendPoint(1);
    }
    
    public void Continue()
    {
        if (!showingText)
            return;
            
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
