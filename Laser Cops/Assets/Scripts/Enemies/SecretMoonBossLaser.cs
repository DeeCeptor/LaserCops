using UnityEngine;
using System.Collections;

public class SecretMoonBossLaser : MonoBehaviour {
    public bool active = false;

    //the direction in which the moon is moving
    public Vector2 moveDirection;
    public float speed = 2f;
    public float range = 5f;

    //time in seconds between changing directions
    public float timeTillChange = 2f;
    public float changeCounter = 0f;
    //when it goes out of it's bounds it will change back every 1 second
    public float changeWhenOutOfRangeTime = 1f;
    public float outOfRangeCounter = 0f;

    //the bounds for the moon to stay in
    float yTopOfScreen;
    float yBottomOfScreen;
    float xRightOfScreen;
    float xLeftOfScreen;

    public void Start() {
        changeCounter = Time.time + timeTillChange;
        outOfRangeCounter = Time.time + changeWhenOutOfRangeTime;
        
        Vector3 minScreenBounds = new Vector3(transform.position.x- range, transform.position.y - range, 0);
        Vector3 maxScreenBounds = new Vector3(transform.position.x + range, transform.position.y + range, 0);

        yTopOfScreen = maxScreenBounds.y;
        yBottomOfScreen = minScreenBounds.y;
        xRightOfScreen = maxScreenBounds.x;
        xLeftOfScreen = minScreenBounds.x;
    }
	
	// Update is called once per frame
	void Update () {
	
        if(active)
        {
            MoveActive();
        }
	}

    public void Activate()
    {
        GetComponent<SpriteRenderer>().sortingLayerName = "Enemies";
        int rand = Random.Range(0,2);
        active = true;
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            RayLaserScript laserScript = transform.GetChild(i).gameObject.GetComponent<RayLaserScript>();
            laserScript.active = true;
            if(rand == 0)
            {
                laserScript.bullet_colour = _Colour.Blue;
                laserScript.laserRenderer.SetColors(Color.cyan, Color.cyan);
            }
            else
            {
                laserScript.bullet_colour = _Colour.Pink;
                laserScript.laserRenderer.SetColors(Color.magenta, Color.magenta);
            }

        }
        moveDirection = Random.insideUnitCircle;
        changeCounter = Time.time + timeTillChange;
    }

    public void MoveActive()
    {
        Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
        velocity = moveDirection.normalized * speed * Time.deltaTime;
        //if going offscreen reverse direction
        if (transform.position.y <= yBottomOfScreen || transform.position.y >= yTopOfScreen || transform.position.x >= xRightOfScreen || transform.position.x <= xLeftOfScreen)
        {
            if (outOfRangeCounter < Time.time)
            {
                outOfRangeCounter = Time.time + changeWhenOutOfRangeTime;
                moveDirection = -moveDirection;
                velocity = velocity * -1;
            }
        }

        //change direction if it is time
        if(changeCounter < Time.time)
        {
            changeCounter = Time.time + timeTillChange;
            moveDirection = Random.insideUnitCircle;
        }
        GetComponent<Rigidbody2D>().velocity = velocity;
    }
}
