using UnityEngine;
using System.Collections;

public class bounceBomb : MonoBehaviour {

    public float damageToBoss = 10f;
    public Transform respawnLocation;
    //max speed ball should move as a magnitude of the velocity vector
    public float maxSpeed = 8f;
    //the x Co-ordinate of the left of screen set at the start
    public float xLeftOfScreen;

    //speed bomb will move towards center
    public float centerAttraction = 0.01f;

	// Use this for initialization
	void Start () {
        xLeftOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(0,0,0)).x + GetComponent<SpriteRenderer>().bounds.size.x;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        CheckSpeed();
        PullToCenter();
        if(transform.position.x <= xLeftOfScreen)
        {
            Respawn();
        }
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
        transform.position = respawnLocation.position;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //to do: respawn animation
    }
}
