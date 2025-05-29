using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDay : Interacteble
{
    public override void Interact()
    {
        if (isInConversation)
            return;

        DialogueSystem.ResetConversation();
        isInConversation = true;
        (showPlayer ? PlayerContainer : NpcContainer).SetActive(true);
    }

    public void End()
    {
        GameManager.Instance.currentActionPoints = 0;
        Invoke(nameof(Outro), 10);
    }

    private void Outro()
    {
        FindAnyObjectByType<StoryCanvas>().Outro();
        Invoke(nameof(LoadNewScene), 2);
    }

    private void LoadNewScene()
    {
        SceneManager.LoadScene("MainMap");
    }

}
