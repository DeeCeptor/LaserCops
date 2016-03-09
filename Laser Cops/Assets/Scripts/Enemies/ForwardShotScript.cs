using UnityEngine;
using System.Collections;

public class ForwardShotScript : MonoBehaviour {
    //will shoot forward at regular intervals
    public float shotDelay = 0.5f;
    public float shotCounter;
    public GameObject bullet;
    public bool active = false;
    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (active)
        {
            if (shotCounter < Time.time)
            {
                shotCounter = Time.time + shotDelay;
                shoot();
            }
        }
        else
        {
            checkActive();
        }
    }

    public void checkActive()
    {
        if (GetComponent<SpriteRenderer>().isVisible)
        {
            Activate();
        }
    }

    //activate the turret
    public void Activate()
    {
        active = true;
        shotCounter = Time.time + shotDelay;
    }

    public void shoot()
    {
        GameObject bulletSpawned = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
        BulletScript bulletStats = bulletSpawned.GetComponent<BulletScript>();
        bulletStats.target = transform.position - transform.up;
    }
}
