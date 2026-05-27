using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    bool over = false;
    bool win = false;
    public Player ball;
    public GameObject gameOver;
    public GameObject win3Game;
    public GameObject win2Game;
    public GameObject win1Game;
    public GameObject showMenu;
    public Text scoreText;
    int counrt = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        counrt = ball.getCounter(); // Get the current value of the counter from the Player script
        scoreText.text = ball.transform.position.z.ToString("0"); // Update the score text to show the ball's z position as an integer

        if (ball.transform.position.y < -5  && !over) // Check if the ball has fallen below a certain height
        {
            GameOver();
        }

        if (ball.transform.position.z > 200 && !win) // Check if the ball has reached a certain distance
        {
            if (counrt <= 2 && !win)
            {
                WinGame3Star();
            }

            else if (counrt > 2 && counrt <= 4 && !win)
            {
                WinGame2Star();
            }
            else if (counrt > 4 && !win)
            {
                WinGame1Star();
            }
        }


    }
    public void GameOver()
    {
        over = true; // Set the game over flag to true
        gameOver.SetActive(true); // Activate the "Win" GameObject to show the win screen
        ball.gameObject.SetActive(false); // Deactivate the ball GameObject to hide it from the scene 
        


    }
    public void WinGame3Star()
    {
        win = true; // Set the game over flag to true
        win3Game.SetActive(true); // Activate the "Win" GameObject to show the win screen
        ball.gameObject.SetActive(false); // Deactivate the ball GameObject to hide it from the scene 
    }

    public void WinGame2Star()
    {
        win = true; // Set the game over flag to true
        win2Game.SetActive(true); // Activate the "Win" GameObject to show the win screen
        ball.gameObject.SetActive(false); // Deactivate the ball GameObject to hide it from the scene 
    }

    public void WinGame1Star()
    {
        win = true; // Set the game over flag to true
        win1Game.SetActive(true); // Activate the "Win" GameObject to show the win screen
        ball.gameObject.SetActive(false); // Deactivate the ball GameObject to hide it from the scene 
    }

    public void ShowMenu()
    {
        showMenu.SetActive(true); // Activate the "ShowMenu" GameObject to show the menu screen
        ball.gameObject.SetActive(false); // Deactivate the ball GameObject to hide it from the scene 
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene to restart the game
    }
    
    public void nextLvL()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load the next scene in the build index to go to the next level
    }
    public void stop()
    {
        ShowMenu(); // Call the ShowMenu method to display the menu screen
    }
    public void back()
    {
               SceneManager.LoadScene(0); // Load the scene with index 0, which is typically the main menu scene
    }

    public void playCurrent()
    {
        ball.gameObject.SetActive(true); // Activate the ball GameObject to show it in the scene
        showMenu.SetActive(false); // Deactivate the "ShowMenu" GameObject to hide the menu screen
    }


}

