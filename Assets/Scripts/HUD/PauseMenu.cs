using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    public static bool isPaused = false;
    float previousTimeScale = 1;
    
    //for canvas 
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;
    private void Start() {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {   
        if(Input.GetKeyDown(KeyCode.Escape)){
            TogglePause();
        }
        
    }
    
    public void TogglePause(){
        //when game is running
        if(!isPaused){
            pauseMenu.SetActive(true);
            optionsMenu.SetActive(false);
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            isPaused = true;
        }
        //when the game is already paused
        else{
            pauseMenu.SetActive(false);
            optionsMenu.SetActive(false);
            Time.timeScale = previousTimeScale;
            isPaused = false;
        }
    }
    private void Reset() {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }
    public void QuitGame(){
        Application.Quit();
        Debug.Log("Quit");
    }

    public void BacktoMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    
}
