using System.Collections.Generic;
using UnityEngine;

public abstract class Interacteble : MonoBehaviour
{
    void Awake()
    {
        foreach (var item in FindObjectsByType<Interacter>(FindObjectsSortMode.None))
        {
            item.localDistantSortedObjects.Add(this);
        }
    }

    void OnDestroy()
    {
        foreach (var item in FindObjectsByType<Interacter>(FindObjectsSortMode.None))
        {
            item.localDistantSortedObjects.Remove(this);
        }
    }

    public abstract void Interact();


    private bool _hidden;
    public bool Hidden
    {
        get => _hidden;
        set
        {
            if (_hidden != value)
            {
                _hidden = value;
                if (!_hidden)
                {
                    Show();
                }
                else
                {
                    Hide();
                }
            }
        }
    }

    public float animationDuration = 0.1f;
    public void Show()
    {
        // Pop in with a bounce
        LeanTween.scale(gameObject, Vector3.one, animationDuration)
            .setEase(LeanTweenType.easeOutBack);
    }

    public void Hide()
    {
        // Smoothly shrink out
        LeanTween.scale(gameObject, Vector3.zero, animationDuration)
            .setEase(LeanTweenType.easeInBack);
    }


}
