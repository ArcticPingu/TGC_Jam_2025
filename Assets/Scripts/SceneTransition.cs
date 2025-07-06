using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public string sceneName = "Credits";
    public float fadeDuration = 2f; 

    public Image fadeImage;

    public void StartFade()
    {


        StartCoroutine(FadeInAndChangeScene());
    }

    IEnumerator FadeInAndChangeScene()
    {
        Color startColor = fadeImage.color;
        fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, 1f);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
    }
}
