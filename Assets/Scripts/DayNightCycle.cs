using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance;

    [Header("Time Settings")]
    public float time; // Normalized time (0 to 1)
    public float dayDuration = 60f; // How many seconds a full day lasts

    [Header("Sun Settings")]
    public Light sunLight;
    public Gradient sunColor;
    public Transform sun;

    [Header("Brightness Settings")]
    public float maxIntensity = 1.5f;
    public float minIntensity = 0f;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // Update time from 0 to 1
        time += Time.deltaTime / dayDuration;
        if (time > 1f) time = 0f;

        // Rotate the sun
        float sunAngle = time * 360f - 90f; // Makes sunrise at 0.25, sunset at 0.75
        sun.rotation = Quaternion.Euler(sunAngle, 170f, 0f);

        // Update sun color using gradient
        sunLight.color = sunColor.Evaluate(time);

        // Calculate brightness (intensity) using a custom formula:
        // Peak at noon (time = 0.5), zero at midnight (time = 0 or 1)
        float brightness = Mathf.Clamp01(Mathf.Cos((time - 0.5f) * Mathf.PI * 2f)); // Cos wave from 0 to 1
        sunLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, brightness);
    }
}
