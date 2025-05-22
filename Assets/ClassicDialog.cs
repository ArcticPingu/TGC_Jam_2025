using cherrydev;
using UnityEditor.Rendering;
using UnityEngine;

public class ClassicDialog : Interacteble
{
    [SerializeField] private DialogNodeGraph dialog;
    public override void Interact()
    {
        DialogBehaviour.Instance.StartDialog(dialog);
    }
}
