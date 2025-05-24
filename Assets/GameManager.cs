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

    public AudioSource source;



    void Awake()
    {
        
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        keys.Add("default");
        
        GameObject.Find("Player").transform.position = spawn;

        SceneManager.sceneLoaded += changeScene;
    }

    private void changeScene(Scene arg0, LoadSceneMode arg1)
    {
        GameObject.Find("Player").transform.position = spawn;
        Debug.Log(GameObject.Find("Player").transform.position );
    }

    public bool checkReq(string req)
    {
        if(req == "" || req == null)
        {
            return true;
        }
        else
        {
            Debug.Log(req[0]);

            if(req[0] == '-')
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
}
