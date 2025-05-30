using UnityEngine;

public class VfxManager : MonoBehaviour
{
    public static VfxManager Instance;
    public AudioSource UI;
    public AudioClip[] uiHover,uiClick;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
    }
    public void PlayUIHover()
    {
        UI.PlayOneShot(uiHover[Random.Range(0,uiHover.Length)]);
    }

    public void PlayUIClick()
    {
        UI.PlayOneShot(uiClick[Random.Range(0,uiClick.Length)]);
    }
}
