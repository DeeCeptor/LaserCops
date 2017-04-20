using UnityEngine;
using System.Collections;

public class LaserFireScript : ForwardShotScript
{
    public float laserLength = 60f;


    void Start ()
    {
	
	}


    void FixedUpdate()
    {
        if (!only_shoot_on_command)
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
    }

    public new void shoot()
    {
            GameObject bulletSpawned = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
            bulletSpawned.transform.position = bulletSpawned.transform.position + (transform.up * laserLength);
            bulletSpawned.transform.SetParent(transform);
    }
}
