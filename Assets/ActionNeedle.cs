using UnityEngine;

public class ActionNeedle : MonoBehaviour
{
    void Update()
    {
        Debug.Log(Mathf.Lerp(-20, -340, GameManager.Instance.currentActionPoints * (1f / GameManager.Instance.maxActionPoints)));
        transform.localPosition = new Vector2(-10.5f, Mathf.Lerp(-158, 158, GameManager.Instance.currentActionPoints * (1f / GameManager.Instance.maxActionPoints)));
    }
}
