using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<string> keys;
    public int maxActionPoints;
    public int currentActionPoints;
    public bool skipIntro;

    public int generosityCounter;

    void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;

        keys.Add("default");
        BerryBush.index = 0;

        if (skipIntro)
            FindAnyObjectByType<StoryCanvas>().SkipIntro();
    }


    public bool checkReq(string req)
    {
        if (req == "" || req == null)
        {
            return true;
        }
        else
        {
            Debug.Log(req[0]);

            if (req[0] == '-')
            {
                Debug.Log(req.Split('-')[1]);
                return !keys.Contains(req.Split('-')[1]);
            }
            else
            {
                return keys.Contains(req);
            }

        }
    }

    public void SpendPoint(int amount)
    {
        currentActionPoints -= amount;

        if (currentActionPoints <= 0)
        {
            foreach (var item in FindObjectsByType<Interacteble>(FindObjectsSortMode.None))
            {
                item.interactable = false;
            }

            FindAnyObjectByType<StoryCanvas>().SadEnd();
        }
    }

    public void Generosity()
    {
        FindAnyObjectByType<StoryCanvas>().Generosity(generosityCounter);
    }
}
