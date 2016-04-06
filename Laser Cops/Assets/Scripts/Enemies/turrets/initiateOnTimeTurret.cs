using UnityEngine;
using System.Collections;

//this script will make a turret fire forward after a number of seconds then deactivate later
public class initiateOnTimeTurret : ForwardShotScript {
    //time from when the enemy enters the screen to when the turret becomes active
    public float timeTillShooting = 2f;
    public bool shooting = false;

    //whether the turret should deactivate after a certain number of seconds
    public bool deactivateAfterTime = true;
    float deactivationCounter;
    public float timeTillDeactivation = 5f;
    public Color deactivatedColor = Color.black;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (active)
        {
            if (shotCounter < Time.time && shooting)
            {
                shotCounter = Time.time + shotDelay;
                shoot();
            }

            if(deactivationCounter < Time.time && deactivateAfterTime)
            {
                Deactivate();
            }
        }
        else
        {
            checkActive();
        }
    }

    public new void checkActive()
    {
        if (GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
        {
            Activate();
        }
    }

    public new void Activate()
    {
        active = true;
        shooting = true;
        shotCounter = Time.time + timeTillShooting;
        if(deactivateAfterTime)
        {
            deactivationCounter = Time.time + timeTillDeactivation;
        }
    }

    public void Deactivate()
    {
        shooting = false;
        GetComponent<SpriteRenderer>().color = deactivatedColor;
    }
}
