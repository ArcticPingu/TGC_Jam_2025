using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionNeedle : MonoBehaviour
{
    [SerializeField] private float speed;
    public int preshow;

    public FutureNeedle[] futureNeedles;

    private bool wasPreshowLastFrame = false;

    void Start()
    {
        futureNeedles = FindObjectsByType<FutureNeedle>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    void Update()
    {
        preshow = 0;

        foreach (var item in futureNeedles)
        {
            preshow += item.currentShow;
        }

        float normalized = (float)GameManager.Instance.currentActionPoints / GameManager.Instance.maxActionPoints;
        float target = Mathf.Lerp(-158, 158, normalized) - ((float)preshow / GameManager.Instance.maxActionPoints) * 158 * 2;

        Vector2 currentPos = transform.localPosition;
        float newY;

        newY = Mathf.Clamp(target,-158, 158);

        transform.localPosition = new Vector2(currentPos.x, newY);

        GetComponent<Image>().color = preshow > 0 ? new Color(1,0.5f,0.5f) : Color.white;

        // Store current state for next frame
        wasPreshowLastFrame = preshow > 0;
    }
}
