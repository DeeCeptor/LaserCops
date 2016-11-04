using UnityEngine;
using System.Collections;

public class ActivateOnCallAndSwitchColors : OnlyActivateOnCallTurret {

    //starting shot color, it will switch
    public _Colour shotColour = _Colour.Blue;

    //Timer to switch colors
    public float TimeBetweenSwitches = 4f;
    public float SwitchTimer = 0f;

	// Use this for initialization
	void Start () {
        SwitchTimer = Time.time + TimeBetweenSwitches;
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

        if(SwitchTimer < Time.time)
        {
            if(shotColour == _Colour.Blue)
            {
                shotColour = _Colour.Pink;
                bullet = (GameObject)Resources.Load("enemies/bullets/PinkBullet");
            }
            else if (shotColour == _Colour.Pink)
            {
                shotColour = _Colour.Blue;
                bullet = (GameObject)Resources.Load("enemies/bullets/BlueBullet");
            }

            SwitchTimer = Time.time + TimeBetweenSwitches;
        }
    }
}
