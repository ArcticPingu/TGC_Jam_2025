using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
    }

    public void LoadSettings()
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
        
        var settingsCanvas = FindAnyObjectByType<SettinsCanvas>();
        
        if (settingsCanvas != null)
        {
            Canvas canvas = settingsCanvas.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = Camera.main;
            }
        }

    }
    public void UnloadScene()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Settings"));
    }

}
