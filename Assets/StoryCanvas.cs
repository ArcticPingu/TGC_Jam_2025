using System;
using UnityEngine;

public class StoryCanvas : MonoBehaviour
{
    public void Outro()
    {
        GetComponent<Animator>().Play("Outro");
    }
}
