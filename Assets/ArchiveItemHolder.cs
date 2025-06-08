using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArchiveItemHolder : MonoBehaviour, IPointerEnterHandler
{
    public TextMeshProUGUI text;
    public string desc;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (text != null)
        {
            text.text = desc;
        }
    }

}
