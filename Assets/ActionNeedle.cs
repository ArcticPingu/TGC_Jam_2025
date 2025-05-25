using UnityEngine;

public class ActionNeedle : MonoBehaviour
{
    [SerializeField] private float speed;

    void Update()
    {
        float target = Mathf.Lerp(-158, 158, GameManager.Instance.currentActionPoints * (1f / GameManager.Instance.maxActionPoints));
        transform.localPosition = new Vector2(-10.5f, Mathf.Lerp(transform.localPosition.y, target, Time.deltaTime * speed));
    }
}
