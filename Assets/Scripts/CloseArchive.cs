using UnityEngine;

public class CloseArchive : MonoBehaviour
{
    public void Press()
    {
        ArchiveManager.Instance.UnloadScene();
    }
}
