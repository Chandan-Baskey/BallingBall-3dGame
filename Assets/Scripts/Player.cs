using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody ball;
    public float ballForce;
    [SerializeField] float conrollingForce;
    void Start()
    {
        ball = GetComponent<Rigidbody>();
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("a"))
        {
             ball.AddForce(-conrollingForce* Time.deltaTime, 0, 0); // Add a leftward force to the ball when the "a" key is pressed
        }
        if(Input.GetKey("d"))
        {
             ball.AddForce(conrollingForce * Time.deltaTime, 0, 0); // Add a rightward force to the ball when the "d" key is pressed
        }
        ball.AddForce(0, 0, ballForce * Time.deltaTime); // Add a forward force to the ball at the start of the game
    }
}
