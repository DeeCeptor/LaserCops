using UnityEngine;
using System.Collections;

public class SecretMoonBossLaser : MonoBehaviour {
    public bool active = false;

    //the direction in which the moon is moving
    public Vector2 moveDirection;
    public float speed = 2f;

    //time in seconds between changing directions
    public float timeTillChange = 2f;
    public float changeCounter = 0f;

    //Screen bounds
    public float yTopOfScreen = 0f;
    public float yBottomOfScreen = 0f;
    public float xRightOfScreen = 0f;
    public float xLeftOfScreen = 0f;

    // Use this for initialization
    void Start () {
        Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

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
        active = true;
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            transform.GetChild(i).gameObject.GetComponent<RayLaserScript>().active = true;
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
            moveDirection = -moveDirection;
            velocity = velocity * -1;
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
