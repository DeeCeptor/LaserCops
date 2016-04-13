using UnityEngine;
using System.Collections;

//this class shoots a laser using a raycast and a line renderer that will shoot from the object until it hits a collider or reaches max laser length, damages player on contact
public class RayLaserScript : MonoBehaviour {
    //how long between shots
    public float shotDelay = 5f;
    //how long the shot lasts
    public int shotDuration = 150;
    //gives the player a time at the start of the shot where they see the laser but it cannot harm them
    public int immuneFrames = 10;
    //counter for immunitytime and shot duration
    public int TimeSinceShotCounter = 0;
    //counter for how long between shots
    public float shotCounter;
    public LineRenderer laserRenderer;
    public bool active = false;
    private RaycastHit2D hit;
    public float maxLaserLength = 100f;
    public bool shooting = false;
    //important note this is damage per "tick"
    public float damage = 0.5f;
    //layers to ignore for the raycast
    LayerMask layersToIgnore;

    //true if you want it to disable itself when the player is close
    public bool playerCloseDisable = true;
    //how close the player must be to disable the shot
    public float disableDistance = 2f;

    void Start () {
        
        shotCounter = shotDelay + Time.time;
        layersToIgnore = ~((1<<12)|(1<<13) | (1 << 15));
	}
	
	void FixedUpdate () {
	    if(shotCounter < Time.time)
        {
            shooting = true;
            TimeSinceShotCounter = 0;
            laserRenderer.enabled = true;
            shotCounter = Time.time + shotDelay;
        }

        if(shooting)
        {
            if( TimeSinceShotCounter > shotDuration)
            {
                shooting = false;
                laserRenderer.enabled = false;
            }
            Shoot();
        }
	}

    public void Shoot()
    {
        //see if there is an obstacle in the way
        hit = Physics2D.Raycast(transform.position, -transform.up,float.PositiveInfinity,layersToIgnore);
        //if there is an obstacle then 
        if (hit.collider != null)
        {
            laserRenderer.SetPosition(0, transform.position);
            laserRenderer.SetPosition(1, hit.point);

        }
        else
        {
            laserRenderer.SetPosition(0, transform.position);
            laserRenderer.SetPosition(1, transform.position - (transform.up * maxLaserLength));
        }

        //if the laser is supposed to do damage
        if (TimeSinceShotCounter > immuneFrames)
        {
            if(hit.collider!=null)
            {
                if(hit.collider.gameObject.CompareTag("Player"))
                {
                    hit.collider.gameObject.GetComponent<PlayerController>().TakeHit(damage);
                }
            } 
        }


        TimeSinceShotCounter += 1;
    }
}
