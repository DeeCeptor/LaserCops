using UnityEngine;
using System.Collections;

//an enemy that scrolls down the screen and then freezes in place and rotates
public class ScrollThenStop : basicScrollingEnemyScript
{
    public float timeTillStop = 3f;
    public float stopCounter;
    public bool rotateWhenStopped = true;
    public float rotateSpeed = 1f;
    //set this to true if you want it to go again after it stops
    public bool goAgain = true;
    public float timeTillGoAgain = 5f;
    public bool stopped = false;

    void Start ()
    {
        initiate();
	}


    void FixedUpdate ()
    {
        tether_lightning_cooldown -= Time.deltaTime;

        if (!active)
        {
            moveInactive();
        }
        else
        {
            moveActive();
            if (!stopped)
            {
                if(stopCounter < Time.time)
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
                if(goAgain)
                {
                    if(stopCounter < Time.time)
                    {
                        unfreeze();
                    }
                }
            }
            CheckDeath();
        }
    }

    public new void CheckActive()
    {
        if (GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
        {
            Activate();
        }
    }

    //Freezes screen position
    public void FreezePosition()
    {
        stopCounter = Time.time + timeTillGoAgain;
        stopped = true;
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    public void unfreeze()
    {
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.constraints = RigidbodyConstraints2D.None;
        //rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("MainCamera"))
        {
            Activate();
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (!active)
        {
            if (collider.gameObject.tag.Equals("MainCamera"))
            {
                Activate();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "MainCamera")
        {
            DieOffScreen();
        }
    }

    public new void Activate()
    {
        active = true;
        stopCounter = Time.time + timeTillStop;
    }
}
