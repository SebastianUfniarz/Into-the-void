using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1"); 
    }

    public void QuitGame()
    {
        Debug.Log("Gra zosta�a zamkni�ta");
        Application.Quit();
    }
}
