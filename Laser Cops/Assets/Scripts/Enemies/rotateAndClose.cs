using UnityEngine;
using System.Collections;

public class rotateAndClose : basicArenaEnemy
{
    public bool rotateClockwise = false;
    public float rotationSpeed = 2f;


    void Start ()
    {
        int rand = Random.Range(0, 1);
        if (rand == 0)
        {
            rotateClockwise = true;
        }

        if (rand == 1)
        {
            rotateClockwise = false;
        }
    }


    void FixedUpdate ()
    {
        if (active == false)
        {
            CheckActive();
            moveInactive();
        }
        if(active == true)
        {
            Follow();
            RotateAround();
            CheckDeath();
        }
    }


    public void RotateAround()
    {
        
        Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
        if (rotateClockwise)
        {
            velocity = velocity + (new Vector2(-velocity.y,velocity.x).normalized * rotationSpeed);
        }
        else if(!rotateClockwise)
        {
            velocity = velocity + (new Vector2(-velocity.y,velocity.x).normalized * -rotationSpeed);
        }
        GetComponent<Rigidbody2D>().velocity = velocity;
    }
}
