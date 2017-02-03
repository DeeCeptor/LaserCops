using UnityEngine;
using System.Collections;

public class SideToSide : basicScrollingEnemyScript
{
    public float sideSpeed = 2f;

    //switch direction this often
    public float switchTime = 1f;
    public float switchCounter;

    //true for up down motion false for right left
    public bool upDown = true;

    //indicates whether it will be moving in the positive or negativ direction
    public bool pos = true;


    void Start ()
    {
        //switchCounter = Time.time + switchTime/2;
        switchCounter = switchTime / 2;
    }


    void FixedUpdate ()
    {
        if (!active)
        {
            moveInactive();
        }
        else if (active)
        {
            CheckDeath();
            moveActive();

            switchCounter -= Time.fixedDeltaTime;
            if (switchCounter <= 0)
            {
                pos = !pos;
                switchCounter = switchTime;
            }

            if (upDown)
            {
                if (pos)
                {
                    GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + new Vector2(0, sideSpeed);
                }

                else
                {
                    GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + new Vector2(0, -sideSpeed);
                }
            }
            else
            {
                if (pos)
                {
                    GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + new Vector2(sideSpeed, 0);
                }

                else
                {
                    GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + new Vector2(-sideSpeed, 0);
                }
            }
        }
	}
}
