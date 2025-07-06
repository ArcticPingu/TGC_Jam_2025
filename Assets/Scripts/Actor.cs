using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Actor", menuName = "ScriptableObjects/Actor", order = 1)]
public class Actor : ScriptableObject
{
    public Sprite[] sprites;
    public TMP_FontAsset font; 
}
