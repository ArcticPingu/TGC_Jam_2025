using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isTalking;
    public List<string> keys;
    public Vector2 spawn;
    public Transform player;
    public AudioClip currentVoice;
    public AudioClip pVoice;
    public int maxActionPoints;
    public int currentActionPoints;
    public AudioSource source;
    public bool skipIntro;



    void Awake()
    {

        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        keys.Add("default");


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
    }
}
