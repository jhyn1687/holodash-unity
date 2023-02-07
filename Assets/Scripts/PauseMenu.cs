using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    
    public static bool isPaused = false;
    float previousTimeScale = 1;
    
    //for canvas 
    [SerializeField] GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {   
        if(Input.GetKeyDown(KeyCode.Escape)){
            TogglePause();
        }
        
    }
    
    public void TogglePause(){
        //when game is running
        if(Time.timeScale > 0){
            pauseMenu.SetActive(true);
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            isPaused = true;
        }
        //when the game is already paused
        else if(Time.timeScale == 0){
            pauseMenu.SetActive(false);
            Time.timeScale = previousTimeScale;
            isPaused = false;
        }
    }

    public void QuitGame(){
        Application.Quit();
        Debug.Log("Quit");
    }
}
