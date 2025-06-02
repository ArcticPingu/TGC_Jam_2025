using System;
using UnityEngine;

public class GateObject : MonoBehaviour
{
    public Sprite closeSprite;
    public void Close()
    {
        GetComponent<Collider>().enabled = true;
        GetComponent<SpriteRenderer>().sprite = closeSprite;
    }

    void Start()
    {
        GetComponent<Collider>().enabled = false;
    }


}
