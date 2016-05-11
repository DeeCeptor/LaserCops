﻿using UnityEngine;
using System.Collections;

public class ScrollThenStopBoss : basicScrollingEnemyScript
{
    public float timeTillStop = 3f;
    public float stopCounter;
    public bool rotateWhenStopped = true;
    public float rotateSpeed = 1f;
    //set this to true if you want it to go again after it stops
    public bool goAgain = true;
    public float timeTillGoAgain = 5f;
    public bool stopped = false;
    // Use this for initialization
    void Start()
    {
        initiate();
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
            CheckActiveB();
            moveInactive();
        }
        else
        {
            
            if (!stopped)
            {
                moveActive();
                if (stopCounter < Time.time)
                {
                    FreezePosition();
                }
            }
            else
            {
                if (rotateWhenStopped)
                {
                    transform.Rotate(new Vector3(0, 0, rotateSpeed));
                }
                if (goAgain)
                {
                    if (stopCounter < Time.time)
                    {
                        unfreeze();
                    }
                }
            }
            CheckDeath();
        }
    }

    public void CheckActiveB()
    {
        if (GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
        {
            ActivateB();
        }
    }

    //Freezes screen position
    public void FreezePosition()
    {
        stopCounter = Time.time + timeTillGoAgain;
        stopped = true;
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        //rigid.isKinematic = false;
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        rigid.velocity = Vector3.zero;
    }

    public void unfreeze()
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.constraints = RigidbodyConstraints2D.None;
        //rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void ActivateB()
    {
        
        active = true;
        stopCounter = Time.time + timeTillStop;
    }

}
