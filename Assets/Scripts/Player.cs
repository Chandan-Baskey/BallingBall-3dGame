using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody ball;
    public float ballForce;
    public Text scoreText;
    void Start()
    {
        ball = GetComponent<Rigidbody>();
        

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text =ball.position.z.ToString("0"); // Update the score text to show the ball's z position as an integer
        if (Input.GetKey("a"))
        {
             ball.AddForce(-ballForce, 0, 0); // Add a leftward force to the ball when the "a" key is pressed
        }
        if(Input.GetKey("d"))
        {
             ball.AddForce(ballForce, 0, 0); // Add a rightward force to the ball when the "d" key is pressed
        }
        ball.AddForce(0, 0, ballForce); // Add a forward force to the ball at the start of the game
    }
}
