using UnityEngine;
using System.Collections;

//enemy script for an enemy that will rotate around the players and attempt to attack from the side
public class circleStrafer : scrollThenChase {

    public bool circling = false;
    public float circlingSpeed = 2f;
    public bool charging = false;
    public float chargeSpeed = 7f;
    public Vector3 chargeDir;
    public bool rotateClockwise;
    public float flankTolerance = 0.2f;
    public float chargeDuration = 1f;
    public float chargeCounter;

	// Use this for initialization
	void Start () {
        int rand = Random.Range(0,1);
        if(rand == 0)
        {
            rotateClockwise = false;
        }
        else
        {
            rotateClockwise = true;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!active)
        {
            CheckActive();
            moveInactive();
        }
        else
        {
            CheckDeath();

            if (!stopped)
            {
                moveActive();
                if (stopCounter < Time.time)
                {
                    stopped = true;
                    circling = true;
                }
            }
            else
            {
                Chase();
                if(circling)
                {
                    Circle();
                    if(transform.position.x <= playerToTrack.position.x + flankTolerance && transform.position.x >= playerToTrack.position.x - flankTolerance)
                    {
                        chargeDir = playerToTrack.position - transform.position;
                        charging = true;
                        circling = false;
                        chargeCounter = Time.time + chargeDuration;
                    }
                }
                else if(charging)
                {
                    Charge();
                    if(chargeCounter < Time.time)
                    {
                        charging = false;
                        circling = true;
                    }
                }

                if (goAgain)
                {
                    if (stopCounter < Time.time)
                    {
                        unfreeze();
                        speed = speedAfterStop;
                        goAgain = false;
                    }
                }
            }
        }
    }

    public void Charge()
    {
        GetComponent<Rigidbody2D>().velocity = chargeDir.normalized * chargeSpeed;
    }

    public void Circle()
    {
        Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
        if (rotateClockwise)
        {
            velocity = velocity + (new Vector2(-velocity.y, velocity.x).normalized * circlingSpeed);
        }
        else if (!rotateClockwise)
        {
            velocity = velocity + (new Vector2(-velocity.y, velocity.x).normalized * -circlingSpeed);
        }
        GetComponent<Rigidbody2D>().velocity = velocity;
    }


}
