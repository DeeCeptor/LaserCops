using UnityEngine;
using System.Collections;

//to be used for the gunship bosses strafing runs
public class shipStrafe : MonoBehaviour {

    public float speed = 3f;
    public direction currentTravelDirection = direction.left;
    public BossHealthScript Health;
    public bool stopped = false;
    public float changeDirectionCounter = 0f;
    public float changeDirectionTime = 10f;
    public float stopDelay;
    public float stopTimer;
    public float stayStoppedDelay = 10f;
    //the parent of turrets which are currently active
    public Transform turretParent;
    public int currentStage = 1;

    public Vector2 desired_velocity = Vector2.zero;
    // Use this for initialization
    void Start () {
        changeDirectionCounter = Time.time + changeDirectionTime;
        stopDelay = 1.5f * changeDirectionTime;
        stopTimer = Time.time + stopDelay;
        turretParent = transform.FindChild("Stage1Turrets");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(currentStage == 1)
        {
            Stage1Update();
        }
	}

    public void ChangeDirections()
    {
        transform.Rotate(0,0,180);
        for (int i = 0; i < turretParent.childCount;i++)
        {
            turretParent.GetChild(i).RotateAround(turretParent.GetChild(i).position, Vector3.forward, 180);
        }

        if (currentTravelDirection == direction.left)
        {
            currentTravelDirection = direction.right;
        }
        else if (currentTravelDirection == direction.right)
        {
            currentTravelDirection = direction.left;
        }
    }

    public void moveActive()
    {
        if (currentTravelDirection == direction.left)
        {
            desired_velocity = new Vector2(-speed, 0);
            GetComponent<Rigidbody2D>().velocity = desired_velocity;
        }
        else if (currentTravelDirection == direction.up)
        {
            desired_velocity = new Vector2(0, speed);
            GetComponent<Rigidbody2D>().velocity = desired_velocity;
        }
        else if (currentTravelDirection == direction.right)
        {
            desired_velocity = new Vector2(speed, 0);
            GetComponent<Rigidbody2D>().velocity = desired_velocity;
        }
        else if (currentTravelDirection == direction.down)
        {
            desired_velocity = new Vector2(0, -speed);
            GetComponent<Rigidbody2D>().velocity = desired_velocity;
        }
    }

    public void changeForms()
    {
        if (currentStage == 1)
        {
            currentStage = 2;
            turretParent.gameObject.SetActive(false);
            turretParent = transform.FindChild("Stage2Turrets");
            turretParent.gameObject.SetActive(true);
        }

    }

    public void Stage1Update()
    {
        if (!stopped)
        {
            if (changeDirectionCounter < Time.time)
            {
                changeDirectionCounter = changeDirectionTime + Time.time;
                ChangeDirections();
            }
            moveActive();
            if (stopTimer < Time.time)
            {
                stopDelay = 2f * changeDirectionTime;
                stopTimer = Time.time + stayStoppedDelay;
                stopped = true;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else if (stopped)
        {
            if (stopTimer < Time.time)
            {
                changeDirectionCounter = (changeDirectionTime * 0.5f) + Time.time;
                stopped = false;
                stopTimer = Time.time + stopDelay;
            }
        }
    }

    public void Stage2Update()
    {

    }

}
