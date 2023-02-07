using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour {
    public void OnResetGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}