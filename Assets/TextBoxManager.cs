using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public List<string> textToDisplay;
    public float textSpeed = 0.05f;

    public float waitAfterText;

    public int currentTextIndex = 0;
    private bool isFillingText = false;

    public bool auto;
    public bool deactivateParentAfterFinish;
    public bool newLine;

    public AudioSource audioTypeSound;

    public UnityEvent onFinish;

    public bool audioVariations;

    private float lastTick;
    public string AutoText;
    public bool AutoSplice;
    public bool animationStartUp;
    public float maxSize;
    public float displaysment;


    private void Start()
    {
        if(AutoSplice)
        {
            string[] valuesArray = AutoText.Split(';');
            textToDisplay = new List<string>(valuesArray);
        }

        StartCoroutine(FillText());
    }

    private void Update()
    {

        if (isFillingText && !auto)
        {
            // Skip the text fill animation.
            StopCoroutine("FillText");
            textBox.text = "";
            textBox.text = textToDisplay[currentTextIndex];
            isFillingText = false;
            StopAllCoroutines();
        }
        else
        {
            // Display the next text when not filling.
            if(!isFillingText)
            {
                NextText();
            }
            
        }
        
    }

    public IEnumerator FillText()
    {
        
        isFillingText = true;
        
        

        if(newLine)
        {
            textBox.text += "\n";

            if(animationStartUp && GetComponent<RectTransform>().sizeDelta.y < maxSize )
            {
                GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta + new Vector2(0,displaysment);
            }
        }
        else
        {
            textBox.text = "";
        }

        string targetText = textToDisplay[currentTextIndex];

        lastTick = Time.time - textSpeed;

        for (int i = 0; i < targetText.Length; )
        {

            for (int n = Mathf.Clamp(Mathf.RoundToInt((Time.time - lastTick) / textSpeed), 1, targetText.Length - (i)); n > 0; n--) //calculate based on tick times
            {

                textBox.text += targetText[i];
                i++;
            }

            
            
            if(audioTypeSound != null && targetText.Length > i && targetText[i] != ' ')
            {
                if(audioVariations)
                {
                    audioTypeSound.pitch = Random.Range(0.92f, 1.08f);
                }

                audioTypeSound.PlayOneShot(audioTypeSound.clip);
            }

            lastTick = Time.time;
            
            if(targetText.Length - (i) == 0)
            {
                i++;
            }

            yield return new WaitForSeconds(textSpeed);
        }
        
        yield return new WaitForSeconds(waitAfterText + Random.Range(waitAfterText/- 3, waitAfterText/ 3));

        isFillingText = false;
    }

    private void NextText()
    {
        currentTextIndex++;

        if (currentTextIndex < textToDisplay.Count)
        {
            StartCoroutine(FillText());
        }
        else
        {
            if(deactivateParentAfterFinish)
            {
                transform.parent.gameObject.SetActive(false);
            }
            
            onFinish.Invoke();
        }

        
        
    }

}
