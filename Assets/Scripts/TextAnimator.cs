using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextAnimator : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private string originalText;

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
        originalText = textMesh.text;
        ParseTags();
        StartCoroutine(AnimateText());
    }

    void Update()
    {
        originalText = textMesh.text;
        ParseTags();
    }

    void ParseTags()
    {
        string parsedText = "";
        Stack<string> tagStack = new Stack<string>();
        int i = 0;

        while (i < originalText.Length)
        {
            if (originalText[i] == '<')
            {
                int closeIndex = originalText.IndexOf('>', i);
                if (closeIndex == -1) break;

                string tagContent = originalText.Substring(i + 1, closeIndex - i - 1);
                bool isClosing = tagContent.StartsWith("/");
                string tagName = isClosing ? tagContent.Substring(1) : tagContent;

                if (!isClosing)
                {
                    tagStack.Push(tagName);
                    tags.Add(new TagInfo { startIndex = parsedText.Length, tag = tagName });
                }
                else
                {
                    if (tagStack.Count > 0 && tagStack.Peek() == tagName)
                    {
                        tagStack.Pop();
                        var lastTag = tags.FindLast(t => t.tag == tagName && t.endIndex == 0);
                        if (lastTag != null)
                        {
                            lastTag.endIndex = parsedText.Length;
                        }
                    }
                }

                i = closeIndex + 1;
            }
            else
            {
                parsedText += originalText[i];
                i++;
            }
        }

        textMesh.text = parsedText;
    }

    IEnumerator AnimateText()
    {
        TMP_TextInfo textInfo = textMesh.textInfo;

        while (true)
        {
            textMesh.ForceMeshUpdate();
            textInfo = textMesh.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible)
                    continue;

                int meshIndex = charInfo.materialReferenceIndex;
                int vertexIndex = charInfo.vertexIndex;

                Vector3[] vertices = textInfo.meshInfo[meshIndex].vertices;

                Vector3 offset = Vector3.zero;

                // Determine if the current character is within a tag range
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
                            offset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1.5f, 1.5f), 0);
                        }
                        break; // Assuming non-overlapping tags
                    }
                }

                for (int j = 0; j < 4; j++)
                {
                    vertices[vertexIndex + j] += offset;
                }
            }

            // Push the updated vertex data to the mesh
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return null;
        }
    }

}
