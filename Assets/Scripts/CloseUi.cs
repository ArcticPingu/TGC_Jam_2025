using UnityEngine;

public class CloseUi : MonoBehaviour
{
    public void Press()
    {
        SettingsManager.Instance.UnloadScene();
    }
}
