using UnityEngine;

public class ArchiveButton : MonoBehaviour
{
    public void Press()
    {
        ArchiveManager.Instance.LoadSettings();
    }
}
