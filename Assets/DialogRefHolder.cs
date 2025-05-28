using TMPro;
using UnityEngine;

public class DialogRefHolder : MonoBehaviour
{
    [Header("UI References")]
    public GameObject PlayerContainer;
    public GameObject NpcContainer;
    public Transform ButtonParent;
    public TMP_Text PlayerText;
    public TMP_Text NpcText;
    public TMP_Text NpcName;
    public static DialogRefHolder Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
    }
}
