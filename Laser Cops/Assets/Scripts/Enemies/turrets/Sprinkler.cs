using UnityEngine;
using System.Collections;
//This script rotates in one direction for a number of seconds before then rotating in the opposite direction for the same period of time
//mostly the script is for turrets but can apply to anything
public class Sprinkler : MonoBehaviour {
    //how long the object will rotate in each direction
    public float rotationDuration = 2f;
    public bool active = false;
    //the counter for when to switch rotation direction the starting value of this will determine how far through it's rotation it starts
    //make it half the rotation duration and the objects starting location will be it's 
    public float rotationCounter = 1f;
    public bool Clockwise = true;
    public float rotationSpeed = 1f;
	// Use this for initialization
	void Start () {
        rotationCounter = rotationCounter + Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
            if (Clockwise)
            {
                transform.Rotate(0, 0, rotationSpeed);
            }
            else
            {
                transform.Rotate(0, 0, -rotationSpeed);
            }

            if(rotationCounter < Time.time)
            {
                Clockwise = !Clockwise;
                rotationCounter = Time.time + rotationDuration;
            }
    }
}
