using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    bool over = false;
    public Player ball;
    public GameObject gameOver;
    public Text scoreText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = ball.transform.position.z.ToString("0"); // Update the score text to show the ball's z position as an integer

        if (ball.transform.position.y < -5  && !over) // Check if the ball has fallen below a certain height
        {
            GameOver();
        }

        if(ball.transform.position.z > 300 && !over) // Check if the ball has reached a certain distance
        {
            GameOver();
        }
    }
    public void GameOver()
    {
        over = true; // Set the game over flag to true
        gameOver.SetActive(true); // Activate the "Win" GameObject to show the win screen
        ball.gameObject.SetActive(false); // Deactivate the ball GameObject to hide it from the scene 
        

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene to restart the game
    }
    
}
