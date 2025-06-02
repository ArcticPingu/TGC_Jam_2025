using TMPro;
using UnityEngine;

public class VersionNumber : MonoBehaviour
{
    public TextMeshProUGUI versionText;
    void Start()
    {
        versionText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        versionText.text = Application.version;
    }
}
