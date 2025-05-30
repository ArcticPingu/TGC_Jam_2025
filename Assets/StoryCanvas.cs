using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class StoryCanvas : MonoBehaviour
{
    public GameObject end;
    public TextMeshProUGUI generosityCounter;
    public GameObject generosityParent;

    public static bool first = true;
    public TextBoxManager textBox;

    public void Outro()
    {
        GetComponent<Animator>().Play("Outro");
    }

    void Awake()
    {
        if (!first)
        {
            textBox.textToDisplay.Add("again..");
        }

        first = false;

        if (globalVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.saturation.value = 0f;
        }
        else
        {
            Debug.LogError("Color Adjustments override not found in Volume.");
        }
    }

    public void SkipIntro()
    {
        GetComponent<Animator>().enabled = false;
    }

    public void Generosity(int counter)
    {
        end.SetActive(true);
        generosityParent.SetActive(true);
        generosityCounter.text = counter + "/1";
    }

    public void SadEnd()
    {
        isFading = true;
    }

    public UnityEngine.Rendering.Volume globalVolume;
    public float fadeDuration = 3f;

    private ColorAdjustments colorAdjustments;
    private float timer = 0f;
    public bool isFading = false;

    void Update()
    {
        if (!isFading || colorAdjustments == null)
            return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / fadeDuration);

        colorAdjustments.saturation.value = Mathf.Lerp(0f, -99.5f, t);

        if (t >= 1f)
        {
            isFading = false;
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        Outro();
        Invoke(nameof(LoadNewScene), 2);
    }

    private void LoadNewScene()
    {
        SceneManager.LoadScene("MainMap");
    }
}
