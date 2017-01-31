using UnityEngine;
using System.Collections;

//to be used for the gunship bosses strafing runs
public class shipStrafe : MonoBehaviour {

    public float speed = 3f;
    public direction currentTravelDirection = direction.left;
    public BossHealthScript Health;
    public bool stopped = false;
    public float stopDelay = 12f;
    public float stopTimer;
    public float stayStoppedDelay = 10f;
    
    public float XLeftOfScreen;
    public float XRightOfScreen;
    public bool goingLeft = true;

    public ConversationManager formChangeConversation1;
    public ConversationManager formChangeConversation2;
    public ConversationManager formChangeConversation3;
    public ConversationManager formChangeConversation4;
    public ConversationManager formChangeConversation5;
    public ConversationManager death_conversation;
    //the parent of turrets which are currently active
    public Transform turretParent;
    public int currentStage = 1;

    GameObject highway;
    PolygonCollider2D box;

    //at what health level to switch stages
    public float healthThreshold;
    public float healthBetweenStages = 500f;

    public Vector2 desired_velocity = Vector2.zero;
    // Use this for initialization
    void Start () {
        stopTimer = Time.time + stopDelay;
        turretParent = transform.FindChild("Stage1Turrets");
        healthThreshold = Health.overallHealth - healthBetweenStages;
        
        box = this.GetComponent<PolygonCollider2D>();
        highway = GameObject.FindGameObjectWithTag("Grid");
        MeshRenderer mesh = highway.GetComponent<MeshRenderer>();

        float screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - box.bounds.extents.x;
        float screenLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + box.bounds.extents.x;

        Vector3 minScreenBounds = new Vector3(screenLeft, mesh.bounds.min.y + box.bounds.extents.y, 0);
        Vector3 maxScreenBounds = new Vector3(screenRight, mesh.bounds.max.y - box.bounds.extents.y, 0);

        XRightOfScreen = maxScreenBounds.x;
        XLeftOfScreen = minScreenBounds.x;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(currentStage == 1)
        {
            Stage1Update();
        }
        else if (currentStage == 2)
        {
            Stage2Update();
        }
        else if (currentStage == 3)
        {
            Stage3Update();
        }
        else if (currentStage == 4)
        {
            Stage4Update();
        }
        else if (currentStage == 5)
        {
            Stage5Update();
        }
        else if (currentStage == 6)
        {
            Stage6Update();
        }

        if (Health.overallHealth < healthThreshold)
        {
            changeForms();
            healthThreshold = healthThreshold - healthBetweenStages;
        }
    }

    public void ChangeDirections()
    {
        //transform.Rotate(0,0,180);


        //for (int i = 0; i < turretParent.childCount;i++)
        //{
        //    turretParent.GetChild(i).RotateAround(turretParent.GetChild(i).position, Vector3.forward, 180);
        //}

        if (currentTravelDirection == direction.left)
        {
            currentTravelDirection = direction.right;
        }
        else if (currentTravelDirection == direction.right)
        {
            currentTravelDirection = direction.left;
        }
        goingLeft = !goingLeft;
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

        if (goingLeft)
        {
            if(transform.position.x < XLeftOfScreen)
            {
                ChangeDirections();
            }
        }
        else
        {
            if (transform.position.x > XRightOfScreen)
            {
                ChangeDirections();
            }
        }
    }

    public void changeForms()
    {
        if (currentStage == 1)
        {
            PlayConversation(formChangeConversation1);
            currentStage = 2;
            turretParent.gameObject.SetActive(false);
            turretParent = transform.FindChild("Stage2Turrets");
            turretParent.gameObject.SetActive(true);

        }
        else if (currentStage == 2)
        {
            PlayConversation(formChangeConversation2);
            currentStage = 3;
            turretParent.gameObject.SetActive(false);
            turretParent = transform.FindChild("Stage3Turrets");
            turretParent.gameObject.SetActive(true);
        }
        else if (currentStage == 3)
        {
            PlayConversation(formChangeConversation3);
            currentStage = 4;
            turretParent.gameObject.SetActive(false);
            turretParent = transform.FindChild("Stage4Turrets");
            turretParent.gameObject.SetActive(true);
        }
        else if (currentStage == 4)
        {
            PlayConversation(formChangeConversation4);
            currentStage = 5;
            turretParent.gameObject.SetActive(false);
            turretParent = transform.FindChild("Stage5Turrets");
            turretParent.gameObject.SetActive(true);
        }
        else if (currentStage == 5)
        {
            PlayConversation(formChangeConversation5);
            currentStage = 6;
            speed = speed / 2;
            turretParent.gameObject.SetActive(false);
            turretParent = transform.FindChild("Stage6Turrets");
            turretParent.gameObject.SetActive(true);
        }
        else
        {
            Die();
        }
    }

    public void Stage1Update()
    {
        if (!stopped)
        {
            moveActive();
            if (stopTimer < Time.time)
            {
                stopTimer = Time.time + stayStoppedDelay;
                stopped = true;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else if (stopped)
        {
            if (stopTimer < Time.time)
            {
                stopped = false;
                stopTimer = Time.time + stopDelay;
            }
        }
    }

    public void Stage2Update()
    {
        if (!stopped)
        {
            moveActive();
            if (stopTimer < Time.time)
            {
                stopTimer = Time.time + stayStoppedDelay;
                stopped = true;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else if (stopped)
        {
            if (stopTimer < Time.time)
            {
                stopped = false;
                stopTimer = Time.time + stopDelay;
            }
        }
    }

    public void Stage3Update()
    {
        if (!stopped)
        {
            moveActive();
            if (stopTimer < Time.time)
            {
                stopTimer = Time.time + stayStoppedDelay;
                stopped = true;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else if (stopped)
        {
            if (stopTimer < Time.time)
            {
                stopped = false;
                stopTimer = Time.time + stopDelay;
            }
        }
    }

    public void Stage4Update()
    {
        if (!stopped)
        {
            moveActive();
            if (stopTimer < Time.time)
            {
                stopTimer = Time.time + stayStoppedDelay;
                stopped = true;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else if (stopped)
        {
            if (stopTimer < Time.time)
            {
                stopped = false;
                stopTimer = Time.time + stopDelay;
            }
        }
    }

    public void Stage5Update()
    {
        if (!stopped)
        {
            moveActive();
            if (stopTimer < Time.time)
            {
                stopTimer = Time.time + stayStoppedDelay;
                stopped = true;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else if (stopped)
        {
            if (stopTimer < Time.time)
            {
                stopped = false;
                stopTimer = Time.time + stopDelay;
            }
        }
    }

    public void Stage6Update()
    {
            moveActive();
    }

    public void PlayConversation(ConversationManager conversation)
    {

        if (conversation != null)
        {
            conversation.transform.SetParent(null);
            conversation.Start_Conversation();
        }
    }

    public void Die()
    {
        PlayConversation(death_conversation);
    }

}
