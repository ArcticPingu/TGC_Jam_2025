using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 _originalScale;
    public float scaleFactor = 1.2f;
    public float scaleDuration = 0.2f; // Duration for the scaling animation
    public ButtonEffectType effectType;
    public bool hoverAudio;
    public bool clickAudio;
    public Image cursorImage;

    private TextMeshProUGUI _textAsset;

    void Start()
    {
        _textAsset = GetComponentInChildren<TextMeshProUGUI>();
        
        if(cursorImage != null)
            cursorImage.enabled = false;
    }
    


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GetComponent<Button>() != null && !GetComponent<Button>().interactable)
        {
            return;
        }

        switch (effectType)
        {
            case ButtonEffectType.Expand:
                // Use LeanTween to scale the button up smoothly
                LeanTween.scale(gameObject, _originalScale * scaleFactor, scaleDuration).setEaseOutBack();
                break;

            case ButtonEffectType.Programmer:
                _textAsset.text = _textAsset.text.Substring(2);
                _textAsset.text = "> " + _textAsset.text;
                break;

            case ButtonEffectType.ShowCursor:
                cursorImage.enabled = true;
                break;

            default:
                Debug.Log("Choose an effect, please.");
                break;
        }

        if (hoverAudio)
        {
            VfxManager.Instance.PlayUIHover();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GetComponent<Button>() != null && !GetComponent<Button>().interactable)
        {
            return;
        }
        
        switch (effectType)
        {
            case ButtonEffectType.Expand:
                // Use LeanTween to scale the button back to its original size smoothly
                LeanTween.scale(gameObject, _originalScale, scaleDuration).setEaseOutBack();
            break;

            case ButtonEffectType.Programmer:
                _textAsset.text = _textAsset.text.Substring(2);
                _textAsset.text = "  " + _textAsset.text;
            break;

            case ButtonEffectType.ShowCursor:
                cursorImage.enabled = false;
            break;

            default:
                Debug.Log("Choose an effect, please.");
                break;
        }
    }

    void OnDisable()
    {
        if (GetComponent<Button>() != null && !GetComponent<Button>().interactable)
        {
            return;
        }
        
        switch (effectType)
        {
            case ButtonEffectType.Expand:
                gameObject.transform.localScale = _originalScale;
            break;

            case ButtonEffectType.Programmer:
                _textAsset.text = _textAsset.text.Substring(2);
                _textAsset.text = "  " + _textAsset.text;
            break;

            case ButtonEffectType.ShowCursor:
                cursorImage.enabled = false;
            break;

            default:
                Debug.Log("Choose an effect, please.");
                break;
        }
    }

    void OnEnable()
    {
        if (clickAudio)
        {
            GetComponent<Button>().onClick.AddListener(() => VfxManager.Instance.PlayUIClick());
        }

        if (_originalScale.magnitude == 0)
        {
            return;
        }

        switch (effectType)
        {
            case ButtonEffectType.Expand:
                transform.localScale = _originalScale;  // Reset scale to the original size
                break;

            case ButtonEffectType.Programmer:
                break;

            case ButtonEffectType.ShowCursor:
                cursorImage.enabled = false;
            break;

            default:
                Debug.Log("Choose an effect, please.");
                break;
        }
    }
}

public enum ButtonEffectType
{
    Expand,
    Programmer,
    ShowCursor
}

