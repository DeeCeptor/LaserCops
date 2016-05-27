using UnityEngine;
using System.Collections;

public class bounceBomb : MonoBehaviour {

    public float damageToBoss = 3f;
    public Vector3 respawnLocation;
    //max speed ball should move as a magnitude of the velocity vector
    public float maxSpeed = 8f;

    //speed bomb will move towards center
    public float centerAttraction = 0.01f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        CheckSpeed();
        PullToCenter();
	}

    public void CheckSpeed()
    {
        Vector3 velocity = GetComponent<Rigidbody2D>().velocity;
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    //this is to keep the ball out of corners
    public void PullToCenter()
    {
        GetComponent<Rigidbody2D>().AddForce(-transform.position.normalized * centerAttraction);
    }

    public void Respawn()
    {

    }
}
