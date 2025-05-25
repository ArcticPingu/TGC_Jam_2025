using cherrydev;
using UnityEditor.Rendering;
using UnityEngine;

public class ClassicDialog : Interacteble
{
    [SerializeField] private DialogNodeGraph dialog;
    DialogBehaviour dialogBehaviour;

    public override void Interact()
    {
        dialogBehaviour.BindExternalFunction("spend", Interaction);
        DialogBehaviour.Instance.StartDialog(dialog);
    }

    void Start()
    {
        dialogBehaviour = FindAnyObjectByType<DialogBehaviour>();
    }

    public void Interaction()
    {
        GameManager.Instance.SpendPoint(1);

        
    }

}
