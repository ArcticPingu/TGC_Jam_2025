using UnityEngine;

public class Map : Interacteble
{
    public GameObject map;
    public void openMap()
    {
        map.SetActive(true);
    }
    public void closeMap()
    {
        map.SetActive(false);
    }
}
