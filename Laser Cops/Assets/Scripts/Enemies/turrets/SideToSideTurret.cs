using UnityEngine;
using System.Collections;

//moves a turret side to side will start by incresing x or y value
public class SideToSideTurret : MonoBehaviour {

    public float speed = 0.25f;
    //how long to move in one direction
    public float limit = 2f;
    public float limitCounter = 0f;
    //whether it has horizontal or vertical back and forth movement
    public bool horizontal = true;
    //whether the object is currently increasing in x or y value
    public bool up = true;

	// Use this for initialization
	void Start () {
        limitCounter = Time.time + limit - limitCounter;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (limitCounter < Time.time)
        {
            up = !up;
            limitCounter = Time.time + limit;

        }

        if (!horizontal)
        {
            if (up)
            {
                transform.Translate(new Vector2(0, speed));
            }

            else
            {
                transform.Translate(new Vector2(0, -speed));
            }
        }
        else
        {
            if (up)
            {
                transform.Translate(new Vector2(speed, 0));
            }

            else
            {
                transform.Translate(new Vector2(-speed, 0));
            }
        }
    }
}
