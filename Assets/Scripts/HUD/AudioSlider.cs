using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioSlider : MonoBehaviour
{
    public float volume;
    public AudioMixer audioMixer;
    public Slider slider;

    void Awake() {
        slider.value = PlayerPrefs.GetFloat("Slider value", 0.75f);
        volume = PlayerPrefs.GetFloat("Slider value", 0.75f);
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }
    void Update() {
        slider.value = PlayerPrefs.GetFloat("Slider value", 0.75f);
        volume = PlayerPrefs.GetFloat("Slider value", 0.75f);
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
    }
    public void OnSliderChange(float newValue) {
        PlayerPrefs.SetFloat("Slider value", newValue);
    }
    
    public void SaveVolume() {
        PlayerPrefs.Save();
    }
}
