using System;
using TMPro;
using UnityEngine;

public class StoryCanvas : MonoBehaviour
{
    public GameObject end;
    public TextMeshProUGUI generosityCounter;
    public GameObject generosityParent;
    public void Outro()
    {
        GetComponent<Animator>().Play("Outro");
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
}
