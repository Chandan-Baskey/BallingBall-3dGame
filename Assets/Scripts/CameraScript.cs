using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform ballPos;
    public Vector3 distanceFromBall; // The distance the camera should maintain from the ball
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        transform.position = ballPos.position - distanceFromBall; // Set the camera's position to the ball's position every frame
    }
}
