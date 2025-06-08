using UnityEngine;
using UnityEngine.SceneManagement;

public class ArchiveManager : MonoBehaviour
{
    public static ArchiveManager Instance;
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
        SceneManager.LoadScene("Archive", LoadSceneMode.Additive);

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
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Archive"));
    }

    public static ArchiveItem[] items = new ArchiveItem[10];
    public void Add(ArchiveItem item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                return;
            }
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                return;
            }
        }
    }
}
