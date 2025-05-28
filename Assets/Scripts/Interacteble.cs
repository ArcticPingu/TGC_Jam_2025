using System.Collections.Generic;
using DialogueGraph.Runtime;
using TMPro;
using UnityEngine;
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

    protected bool isInConversation = false;
    protected bool showPlayer;
    protected bool isPlayerChoosing;
    protected bool shouldShowText;
    protected bool showingText;
    protected string textToShow;
    protected
    void Awake()
    {
        foreach (var item in FindObjectsByType<Interacter>(FindObjectsSortMode.None))
        {
            item.localDistantSortedObjects.Add(this);
        }
    }

    void Start()
    {
        PlayerContainer = DialogRefHolder.Instance.PlayerContainer;
        NpcContainer = DialogRefHolder.Instance.NpcContainer;
        ButtonParent = DialogRefHolder.Instance.ButtonParent;
        PlayerText = DialogRefHolder.Instance.PlayerText;
        NpcText = DialogRefHolder.Instance.NpcText;
        NpcName = DialogRefHolder.Instance.NpcName;
    }

    void OnDestroy()
    {
        foreach (var item in FindObjectsByType<Interacter>(FindObjectsSortMode.None))
        {
            item.localDistantSortedObjects.Remove(this);
        }
    }

    public abstract void Interact();


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
        GameManager.Instance.currentActionPoints--;
    }

}
