using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void startGame()
    { 
        SceneManager.LoadScene(2);
    }

    public void quitGame()
    {
            Application.Quit();
    }

    public void LeveSelect()
    {
        SceneManager.LoadScene(1);
    }
}
