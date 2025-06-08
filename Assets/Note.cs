using UnityEngine;

public class Note : Interacteble
{
    public GameObject note;
    public ArchiveItem item;
    public int id;
    public void TakeNote()
    {
        ArchiveManager.Instance.Add(item);
        note.SetActive(false);
        interactable = false;
    }

    public bool Is1()
    {
        return id == 1;
    }   

    public bool Is2()
    {
        return id == 2;
    }

    public bool Is3()
    {
        return id == 3;
    }
    
    public bool Is4()
    {
        return id == 4;
    }

}
