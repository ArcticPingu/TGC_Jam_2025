using UnityEngine;

[CreateAssetMenu(fileName = "ArchiveItem", menuName = "Assets/ArchiveItem")]
public class ArchiveItem : ScriptableObject
{
    public string id;
    public Sprite sprite;
    public string desc;
}
