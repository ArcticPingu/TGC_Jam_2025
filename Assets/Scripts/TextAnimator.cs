using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextAnimator : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    public static bool animating;
    public static TextAnimator animator;
    public string originalText;
    public float shakeIntensity;
    public float shakeFrequenzy;
    public float typingSpeed = 0.05f;

    private float typingTimer;
    private int visibleCharacterCount;
    private bool isTyping = false;

    [System.Serializable]
    public class TagInfo
    {
        public int startIndex;
        public int endIndex;
        public string tag;
    }

    public List<TagInfo> tags = new List<TagInfo>();

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void ForceParseLocal()
    {
        visibleCharacterCount = 0;
        textMesh.maxVisibleCharacters = 0;
        originalText = textMesh.text;
        ParseTags();
        StartCoroutine(RevealTextCoroutine());
    }

    void ParseTags()
    {
        tags = new List<TagInfo>();

        string parsedText = "";
        Stack<(string tag, int startIndex)> tagStack = new Stack<(string, int)>();
        int i = 0;

        while (i < originalText.Length)
        {
            if (originalText[i] == '<')
            {
                int closeIndex = originalText.IndexOf('>', i);
                if (closeIndex == -1) break;

                string fullTag = originalText.Substring(i, closeIndex - i + 1);
                string tagContent = originalText.Substring(i + 1, closeIndex - i - 1);
                bool isClosing = tagContent.StartsWith("/");
                string tagName = isClosing ? tagContent.Substring(1) : tagContent.Split('=')[0];

                bool isWaveOrShake = tagName == "wave" || tagName == "shake";

                if (isWaveOrShake)
                {
                    if (!isClosing)
                    {
                        tagStack.Push((tagName, parsedText.Length));
                    }
                    else
                    {
                        if (tagStack.Count > 0 && tagStack.Peek().tag == tagName)
                        {
                            var (openTag, startIndex) = tagStack.Pop();
                            tags.Add(new TagInfo { tag = tagName, startIndex = startIndex, endIndex = parsedText.Length });
                        }
                    }

                    // Skip adding <wave> or <shake> tags to final text
                    i = closeIndex + 1;
                }
                else
                {
                    // Keep all other tags
                    parsedText += fullTag;
                    i = closeIndex + 1;
                }
            }
            else
            {
                parsedText += originalText[i];
                i++;
            }
        }

        textMesh.text = parsedText;
        originalText = parsedText;
    }




    IEnumerator RevealTextCoroutine()
    {
        animator = this;
        animating = true;
        isTyping = true;

        visibleCharacterCount = 0;
        textMesh.maxVisibleCharacters = 0;

        while (visibleCharacterCount < originalText.Length)
        {
            visibleCharacterCount++;
            textMesh.maxVisibleCharacters = visibleCharacterCount;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        animating = false;
        animator = null;
    }

    public void Finish()
    {
        visibleCharacterCount = 9999;
        textMesh.maxVisibleCharacters = visibleCharacterCount;
    }

    void Update()
    {
        AnimateText();
    }

    void AnimateText()
    {
        if (!gameObject.activeInHierarchy) return;
        if (!textMesh.IsActive()) return;

        if (textMesh.textInfo.characterCount == 0)
            return;


        TMP_TextInfo textInfo = textMesh.textInfo;
        textMesh.ForceMeshUpdate();
        textInfo = textMesh.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int meshIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            Vector3[] vertices = textInfo.meshInfo[meshIndex].vertices;
            Vector3 offset = Vector3.zero;

            foreach (var tag in tags)
            {
                if (i >= tag.startIndex && i < tag.endIndex)
                {
                    if (tag.tag == "wave")
                    {
                        offset = new Vector3(0, Mathf.Sin(Time.time * 10f + i) * 5f, 0);
                    }
                    else if (tag.tag == "shake")
                    {
                        float time = Time.time * shakeFrequenzy;
                        float perCharSeedX = i * 7.13f + 0.123f;
                        float perCharSeedY = i * 9.41f + 0.987f;

                        float x = (Mathf.PerlinNoise(perCharSeedX, time) - 0.5f) * 2f * shakeIntensity;
                        float y = (Mathf.PerlinNoise(perCharSeedY, time) - 0.5f) * 2f * shakeIntensity * 2f;

                        offset = new Vector3(x, y, 0);
                    }
                    break;
                }
            }

            for (int j = 0; j < 4; j++)
            {
                vertices[vertexIndex + j] += offset;
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }

    void OnEnable()
    {
        originalText = "";
        StartCoroutine(DelayedForceUpdate());
    }

    IEnumerator DelayedForceUpdate()
    {
        yield return null; // Wait one frame
        textMesh.ForceMeshUpdate(); // Rebuild mesh info
        AnimateText(); // Now mesh data will be correct
    }
}
