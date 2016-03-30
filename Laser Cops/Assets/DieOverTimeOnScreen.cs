using UnityEngine;
using System.Collections;

//dies after being on screen for x seconds
public class DieOverTimeOnScreen : MonoBehaviour {

    //speed when offscreen
    public float inactiveSpeed = 1f;
    public direction travelDirection = direction.left;
    public float secondsTillDeath = 3f;
    private float deathCounter = 0f;
    //how quickly to move on screen
    public float activeSpeed = 1f;
    public bool active = false;
    public float timeToMoveOnScreen = 1f;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!active)
        {
            CheckActive();
            moveInactive();
        }
        else
        {
            if (deathCounter > Time.time + timeToMoveOnScreen)
            {
                moveActive();
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            }

            if(deathCounter < Time.time)
            {
                Destroy(gameObject);
            }
        }
    }

    public void CheckActive()
    {
        if (GetComponent<SpriteRenderer>().isVisible)
        {
            Activate();
        }
    }

    public void moveInactive()
    {
        if (travelDirection == direction.left)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-inactiveSpeed, 0);
        }
        else if (travelDirection == direction.up)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, inactiveSpeed);
        }
        else if (travelDirection == direction.right)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(inactiveSpeed, 0);
        }
        else if (travelDirection == direction.down)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -inactiveSpeed);
        }
    }

    public void moveActive()
    {
        if (travelDirection == direction.left)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-activeSpeed, 0);
        }
        else if (travelDirection == direction.up)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, activeSpeed);
        }
        else if (travelDirection == direction.right)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(activeSpeed, 0);
        }
        else if (travelDirection == direction.down)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -activeSpeed);
        }
    }

    public void Activate()
    {
        active = true;
        deathCounter = secondsTillDeath + Time.time;

    }
}
