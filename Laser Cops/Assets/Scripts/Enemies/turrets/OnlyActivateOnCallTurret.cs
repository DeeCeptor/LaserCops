using UnityEngine;
using System.Collections;

public class OnlyActivateOnCallTurret : ForwardShotScript {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active && !only_shoot_on_command)
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
