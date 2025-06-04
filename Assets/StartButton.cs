using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("MainMap");
    }

    public void Settings()
    {
        SettingsManager.Instance.LoadSettings();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
