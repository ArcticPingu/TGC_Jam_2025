using UnityEngine;
using UnityEngine.UI;

public class ArchiveFrame : MonoBehaviour
{
    public ArchiveManager manager;
    public Image[] itemFrames;

    void Awake()
    {
        manager = FindAnyObjectByType<ArchiveManager>();
    }

    void Update()
    {
        for (int i = 0; i < itemFrames.Length; i++)
        {
            itemFrames[i].enabled = ArchiveManager.items[i] != null;

            if (ArchiveManager.items[i] != null)
            {
                itemFrames[i].sprite = ArchiveManager.items[i].sprite;
                itemFrames[i].GetComponent<ArchiveItemHolder>().desc = ArchiveManager.items[i].desc;
            }
        }
    }
    

}
