using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDay : Interacteble
{
    public void End()
    {
        GameManager.Instance.SpendPoint(999);
    }
}
