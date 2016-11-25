using UnityEngine;
using System.Collections;

public class RandomMovingScrollingEnemyScript : basicScrollingEnemyScript {

    public float maxChangeTime = 4f;
    public float minChangeTime = 2f;
    private float changeCounter = 0f;
    public float yTopOfScreen = 0f;
    public float yBottomOfScreen = 0f;
    public float xRightOfScreen = 0f;
    public float xLeftOfScreen = 0f;

    GameObject highway;
    BoxCollider2D box;
    // Use this for initialization
    void Start () {

        box = this.GetComponent<BoxCollider2D>();
        highway = GameObject.FindGameObjectWithTag("Grid");
        MeshRenderer mesh = highway.GetComponent<MeshRenderer>();

        float screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width,0,0 )).x - box.bounds.extents.x;
        float screenLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + box.bounds.extents.x;

        Vector3 minScreenBounds = new Vector3(screenLeft, mesh.bounds.min.y + box.bounds.extents.y, 0);
        Vector3 maxScreenBounds = new Vector3(screenRight, mesh.bounds.max.y - box.bounds.extents.y, 0);

        yTopOfScreen = maxScreenBounds.y;
        yBottomOfScreen = minScreenBounds.y;
        xRightOfScreen = maxScreenBounds.x;
        xLeftOfScreen = minScreenBounds.x;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("MainCamera"))
        {
            changeCounter = Time.time + Random.Range(minChangeTime, maxChangeTime);
            Activate();
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (!active)
        {
            if (collider.gameObject.tag.Equals("MainCamera"))
            {
                changeCounter = Time.time + Random.Range(minChangeTime, maxChangeTime);
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

    // Update is called once per frame
    void FixedUpdate () {
        tether_lightning_cooldown -= Time.deltaTime;

        if (!active)
        {
            moveInactive();
        }
        else
        {
            CheckDeath();
            moveActive();
            if(changeCounter < Time.time)
            {
                changeCounter = Time.time + Random.Range(minChangeTime, maxChangeTime);
                changeDirection();
            }
        }
    }

    public void changeDirection()
    {
        int rand = Random.Range(0,4);

        if (rand == 0)
        {
            travelDirection = direction.left;
        }

        else if (rand == 1)
        {
            travelDirection = direction.down;
        }

        else if (rand == 2)
        {
            travelDirection = direction.right;
        }

        else if (rand == 3)
        {
            travelDirection = direction.up;
        }
    }

    public new void moveActive()
    {
        if (travelDirection == direction.left)
        {
            desired_velocity = new Vector2(-speed, 0);
            GetComponent<Rigidbody2D>().velocity = desired_velocity;
            if(transform.position.x <= xLeftOfScreen)
            {
                travelDirection = direction.right;
                desired_velocity = desired_velocity * -1;
            }
        }
        else if (travelDirection == direction.up)
        {
            desired_velocity = new Vector2(0, speed);
            GetComponent<Rigidbody2D>().velocity = desired_velocity;
            if (transform.position.y >= yTopOfScreen)
            {
                travelDirection = direction.down;
                desired_velocity = desired_velocity * -1;
            }
        }
        else if (travelDirection == direction.right)
        {
            desired_velocity = new Vector2(speed, 0);
            GetComponent<Rigidbody2D>().velocity = desired_velocity;
            if (transform.position.x >= xRightOfScreen)
            {
                travelDirection = direction.left;
                desired_velocity = desired_velocity * -1;
            }
        }
        else if (travelDirection == direction.down)
        {
            desired_velocity = new Vector2(0, -speed);
            GetComponent<Rigidbody2D>().velocity = desired_velocity;
            if (transform.position.y <= yBottomOfScreen)
            {
                travelDirection = direction.up;
                desired_velocity = desired_velocity * -1;
            }
        }
    }
}
