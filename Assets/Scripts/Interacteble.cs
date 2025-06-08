using System;
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
    protected GameObject CodeContainer;
    protected Transform PlayerButtonParent;
    protected Transform CodeButtonParent;

    protected TMP_Text PlayerText;
    protected TMP_Text NpcText;
    protected TMP_Text NpcName;
    protected TMP_Text CodeText;
    protected Image npcImage;

    public PlayerController talker;

    protected bool isInConversation = false;
    public ShowField showPlayer;
    protected bool isPlayerChoosing;
    protected bool shouldShowText;
    protected bool showingText;
    protected string textToShow;
    [SerializeField] private bool skipPlayerText;

    public string CostId;
    public bool interactable;
    public int emotionIndex = 0;

    public void SetEmotionIndex(int index)
    {
        emotionIndex = index;
    }

    [Serializable]
    public enum ShowField
    {
        player,
        npc,
        code,
    }
    void Awake()
    {
        foreach (var item in FindObjectsByType<Interacter>(FindObjectsSortMode.None))
        {
            item.localDistantSortedObjects.Add(this);
        }

        interactable = true;
        isInConversation = false;
    }

    public bool CheckFlag(string id)
    {
        return InventoryManager.Instance.flags.Contains(id);
    }

    public void AddFlag(string id)
    {
        InventoryManager.Instance.flags.Add(id);
    }

    void Start()
    {
        PlayerContainer = DialogRefHolder.Instance.PlayerContainer;
        NpcContainer = DialogRefHolder.Instance.NpcContainer;
        PlayerButtonParent = DialogRefHolder.Instance.PlayerButtonParent;
        CodeButtonParent = DialogRefHolder.Instance.CodeButtonParent;
        PlayerText = DialogRefHolder.Instance.PlayerText;
        NpcText = DialogRefHolder.Instance.NpcText;
        NpcName = DialogRefHolder.Instance.NpcName;
        npcImage = DialogRefHolder.Instance.npcImage;
        CodeContainer = DialogRefHolder.Instance.CodeContainer;
        CodeText = DialogRefHolder.Instance.CodeText;
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

        talker = player;

        talker.canMove = false;

        if (DialogueSystem.IsCurrentNpc())
        {
            showPlayer = ShowField.npc;
        }
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
            switch (showPlayer)
            {
                case ShowField.player:
                    PlayerContainer.SetActive(true);
                    PlayerText.gameObject.SetActive(true);
                    PlayerText.text = textToShow;
                    PlayerText.GetComponent<TextAnimator>().ForceParseLocal();
                    break;
                case ShowField.npc:
                    NpcContainer.SetActive(true);
                    NpcText.gameObject.SetActive(true);
                    NpcText.text = textToShow;
                    NpcText.GetComponent<TextAnimator>().ForceParseLocal();
                    break;
                case ShowField.code:
                    CodeContainer.SetActive(true);
                    CodeText.text = textToShow;
                    break;
            }

            showingText = true;
            shouldShowText = false;
        }

        if (!showingText)
        {
            Debug.Log(DialogueSystem);

            if (DialogueSystem.IsConversationDone())
            {
                switch (showPlayer)
                {
                    case ShowField.player:
                        PlayerText.text = "";
                        textToShow = "";
                        PlayerContainer.SetActive(false);

                        break;
                    case ShowField.npc:
                        textToShow = "";
                        NpcText.text = "";
                        NpcContainer.SetActive(false);

                        break;
                    case ShowField.code:
                        CodeContainer.SetActive(false);
                        break;
                }

                // Reset state
                isInConversation = false;
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
                npcImage.sprite = container.sprites[emotionIndex];
                NpcText.font = container.font;
                shouldShowText = true;
                Debug.Log(currentActor);
                NpcName.text = currentActor.Name;
                textToShow = DialogueSystem.ProgressNpc();
            }
            else
            {
                isPlayerChoosing = true;

                Transform ButtonParent;

                if (showPlayer == ShowField.code)
                {
                    CodeContainer.SetActive(true);
                    ButtonParent = CodeButtonParent;
                }
                else
                {
                    PlayerContainer.SetActive(true);
                    ButtonParent = PlayerButtonParent;
                }
                

                List<ConversationLine> lines = DialogueSystem.GetCurrentLines();

                ushort count = 0;
                int lastIndex = 0;


                for (int i = 0; i < 10; i++)
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

                        }

                        // 
                        Button button = ButtonParent.GetChild(i).GetComponentInChildren<Button>();
                        button.onClick.RemoveAllListeners(); // Remove existing listeners
                        int index = i;
                        button.onClick.AddListener(() => Click(index));

                        lastIndex = i;
                        PlayerText.GetComponent<TextAnimator>().ForceParseLocal();

                    }
                }

                if (count == 1)
                {
                    Click(lastIndex);
                }
                else if (count == 0)
                {
                    Debug.LogWarning("skip player dialogue");

                    Click(0);
                    Update();
                    Continue();
                    Update();
                    Continue();
                }
                else
                {
                    if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
                    {
                        EventSystem.current.SetSelectedGameObject(ButtonParent.GetChild(0).gameObject);
                        Debug.Log("Controller Select");
                    }
                    else
                    {
                        EventSystem.current.SetSelectedGameObject(null);
                    }
                }

            }
        }
    }

    public void Click(int index)
    {
        for (int i = 0; i < 10; i++)
        {
            PlayerButtonParent.GetChild(i).gameObject.SetActive(false);
            CodeButtonParent.GetChild(i).gameObject.SetActive(false);
        }

        isPlayerChoosing = false;
        shouldShowText = true;

        if (skipPlayerText)
        {
            if (showPlayer == ShowField.code)
            {
                textToShow += DialogueSystem.ProgressSelf(index) + " ";
                showingText = true;
                Update();
            }
            else
            {
                DialogueSystem.ProgressSelf(index);
            }
            
            Continue();
        }
        else
        {
            
            textToShow = DialogueSystem.ProgressSelf(index);
        }
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

        if (TextAnimator.animating && !skipPlayerText)
        {
            TextAnimator.animator.Finish();
            
            Debug.Log("SkippedAnimation");
            return;
        }

        Debug.Log("continue");


        showingText = false;

        ShowField nextShowPlayer;

        if (!DialogueSystem.IsConversationDone() && DialogueSystem.IsCurrentNpc())
        {
            nextShowPlayer = ShowField.npc;
        }
        else
        {
            if (skipPlayerText)
            {
                nextShowPlayer = ShowField.code;
            }
            else
            {
                nextShowPlayer = ShowField.player;
            }
        }

        if (DialogueSystem.IsConversationDone() || nextShowPlayer != showPlayer)
        {
            switch (showPlayer)
            {
                case ShowField.player:
                    PlayerText.text = "";
                    textToShow = "";
                    PlayerContainer.SetActive(false);

                    break;
                case ShowField.npc:
                    textToShow = "";
                    NpcText.text = "";
                    NpcContainer.SetActive(false);

                    break;
                case ShowField.code:
                    CodeContainer.SetActive(false);
                    Debug.Log("code hide");
                    break;
            }
        }

        showPlayer = nextShowPlayer;

        Update();
    }

    public void ActivateSkip()
    {
        skipPlayerText = true;
    }

    public void DeactivateSkip()
    {
        skipPlayerText = false;
        CodeText.text = "";
    }


}
