using UnityEngine;

public class StreetLamp : MonoBehaviour
{
    public static bool on = true;
    public static void ChangeStatus(bool enabled)
    {
        if (enabled)
        {
            StreetLamp[] allLights = FindObjectsByType<StreetLamp>(FindObjectsSortMode.None);

            foreach (var light in allLights)
            {
                light.GetComponentInChildren<Light>().enabled = true;
                light.GetComponent<SpriteRenderer>().materials[0].SetColor("_color", new Color(191, 130, 60) * 3f);
            }

            on = true;

        }
        else
        {
            StreetLamp[] allLights = FindObjectsByType<StreetLamp>(FindObjectsSortMode.None);

            foreach (var light in allLights)
            {
                light.GetComponentInChildren<Light>().enabled = false;
                light.GetComponent<SpriteRenderer>().materials[0].SetColor("_color", new Color(191f/255f, 130f/255f, 60f/255f));
            }

            on = false;
        }
    }


    public static bool GetStatus()
    {
        return on;
    }
}
