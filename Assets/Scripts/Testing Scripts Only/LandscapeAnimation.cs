using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeAnimation : MonoBehaviour
{
    private Vector3 startAngle;   //Reference to the object's original angle values

    private float rotationSpeed = 0.01f;  //Speed variable used to control the animation

    private float rotationOffset = 20f; //Rotate by 50 units

    private float finalAngle;  //Keeping track of final angle to keep code cleaner

    void Start()
    {
        startAngle = transform.eulerAngles;  // Get the start position
    }

    void FixedUpdate()
    {
        finalAngle = (float) startAngle.x + (float)Math.Sin(Environment.TickCount*rotationSpeed)*rotationOffset;  //Calculate animation angle
        transform.eulerAngles = new Vector3(startAngle.x, finalAngle, startAngle.z); //Apply new angle to object
    }
}
