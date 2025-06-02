using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    public void Press()
    {
        SettingsManager.Instance.LoadSettings();
    }
}
