using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance;

    [Header("Time Settings")]
    public float time; // Smoothed "day curve" time (0 to 1)
    public float targetNormalizedAP;
    public float smoothedAP;
    public float speed = 1;

    [Header("Sun Settings")]
    public Light sunLight;
    public Gradient sunColor;
    public Transform sun;

    [Header("Brightness Settings")]
    public float maxIntensity = 1.5f;
    public float minIntensity = 0f;

    [Header("Sun Movement Settings")]
    public float minElevationAngle = 10f; // Horizon
    public float maxElevationAngle = 70f; // Peak
    public float startAzimuth = 55f;      // Sunrise
    public float endAzimuth = -55f;       // Sunset

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // Smooth AP input to avoid jumps
        targetNormalizedAP = Mathf.Clamp01((float)GameManager.Instance.currentActionPoints / GameManager.Instance.maxActionPoints);
        smoothedAP = Mathf.Lerp(smoothedAP, targetNormalizedAP, Time.deltaTime * speed);

        // Parabolic curve based on smoothed AP (not jumpy)
        time = Mathf.Sin(smoothedAP * Mathf.PI);

        // Elevation (X axis)
        float elevation = Mathf.Lerp(minElevationAngle, maxElevationAngle, time);

        // Azimuth (Y axis) from smoothedAP
        float azimuth = Mathf.Lerp(startAzimuth, endAzimuth, smoothedAP);

        // Apply rotation
        sun.rotation = Quaternion.Euler(elevation, azimuth, 0f);

        // Intensity and color
        float brightness = Mathf.Lerp(minIntensity, maxIntensity, time);
        sunLight.intensity = brightness;
        sunLight.color = sunColor.Evaluate(time);

        // Shader
        Shader.SetGlobalVector("_Sun", sun.transform.forward);
    }
}
