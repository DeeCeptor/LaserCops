using UnityEngine;
using System.Collections;

public class OnlyActivateOnCallTurret : ForwardShotScript {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active)
        {
            if (shotCounter < Time.time)
            {
                shotCounter = Time.time + shotDelay;
                shoot();
            }
        }
        else
        {
        }
    }
}
