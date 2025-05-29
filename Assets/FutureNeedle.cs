using UnityEngine;
using UnityEngine.EventSystems;

public class FutureNeedle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool showFuture;
    public int currentShow;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (showFuture)
        {
            currentShow = 1;

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (showFuture)
        {
            currentShow = 0;
        }
    }

    void OnDisable()
    {
        currentShow = 0;
    }
}
