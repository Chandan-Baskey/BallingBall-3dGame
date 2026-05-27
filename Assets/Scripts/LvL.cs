using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LvL : MonoBehaviour
{
   public void LvL1()
    {
        SceneManager.LoadScene(2);
    }

    public void LvL2()
    {
        SceneManager.LoadScene(3);
    }

    public void LvL3()
    {
        SceneManager.LoadScene(4);
    }

    public void home()
    {
        SceneManager.LoadScene(0);
    }

    public void startGame()
    {
        SceneManager.LoadScene(2);
    }
}
