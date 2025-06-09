using UnityEngine;

public class UITutotial : MonoBehaviour
{
    public static bool once;
    void Start()
    {
        if (once)
        {
            gameObject.SetActive(true);
        }

        once = true;
    }
}
