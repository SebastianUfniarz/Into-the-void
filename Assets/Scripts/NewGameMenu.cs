using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameMenu : MonoBehaviour
{
    //Placeholder: tu bedzie trzeba dodac wybieranie roznych postaci itd
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }
}