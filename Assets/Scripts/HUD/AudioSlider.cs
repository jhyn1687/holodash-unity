using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioSlider : MonoBehaviour
{
    public AudioMixer audioMixer;   
    
    public void SetVolume (float volume){
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20); 
    }
    /* public Slider xslider;
 
 
     void Start()
     {
        xslider.value = PlayerPrefs.GetFloat("volume");
        xslider.SetLevel(PlayerPrefs.GetFloat("volume"));
 
     }
 
     void Update()
     {
         PlayerPrefs.SetFloat("volume", xslider.value);
     
 
     } */
   
    /* void Awake ()
    {
        musicSlider.value = PlayerPrefs.GetFloat ("volume");
        audioMixer.SetFloat("volume", musicSlider.value);
    }
    public void SetMusicVolume (float musicVolume)
    {
        audioMixer.SetFloat("volume", musicVolume);
        PlayerPrefs.SetFloat ("volume", musicVolume);
    }  */
        
/*     private void OnDisable () {
         PlayerPrefs.Save();
     } 
 */  
    
}
