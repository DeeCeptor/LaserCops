using UnityEngine;
using System.Collections;

public class SideToSideGunship : basicScrollingEnemyScript
{
    public float sideSpeed = 2f;

    //switch direction this often
    public float switchTime = 1f;
    public float switchCounter;

    //true for up down motion false for right left
    public bool upDown = true;

    //indicates whether it will be moving in the positive or negativ direction
    public bool pos = true;
    // Use this for initialization
    void Start()
    {
        switchCounter = Time.time + switchTime / 2;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

    }

    void OnCollisionStay2D(Collision2D collision)
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!active)
        {
            CheckActive();
            moveInactive();
        }

        else if (active)
        {
            CheckDeath();
            moveActive();

            if (switchCounter < Time.time)
            {
                pos = !pos;
                switchCounter = Time.time + switchTime;

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
