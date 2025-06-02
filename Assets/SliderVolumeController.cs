using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SliderVolumeController : MonoBehaviour
{
    public AudioMixer audioMixer; // Reference to the Audio Mixer
    public string parameterName; // Name of the parameter (e.g., "SoundVolume" or "FXVolume")
    private Slider slider;

    public void Start()
    {
        Initiate();
    }

    public void Initiate()
    {
        slider = GetComponent<Slider>();
        
        if(parameterName == null || parameterName == "")
        {
            Destroy(gameObject);
        }

        float savedVolume = PlayerPrefs.GetFloat(parameterName, 1f);
        slider.value = savedVolume;
        ChangeVolume(savedVolume);
        slider.onValueChanged.AddListener(ChangeVolume);
    }

    private void ChangeVolume(float value)
    {    
        // Convert linear volume value to logarithmic dB scale
        float volumeInDecibels = 20f * Mathf.Log10(value);
        
        if(value == 0)
        {
            volumeInDecibels = -80;
        }

        // if(parameterName == "main")
        // {

        
        // }
        // else
        // {
            PlayerPrefs.SetFloat(parameterName, value);
            audioMixer.SetFloat(parameterName, volumeInDecibels);
        // }
    }
}
