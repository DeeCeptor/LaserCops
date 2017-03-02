using UnityEngine;
using System.Collections;

//this script was copied from the laser spawn script so some variable names may be confusing 
//this script does the same thing as the laser spawn script except it does not set the spawned object to be it's child
public class pillarSpawnerScript : ForwardShotScript
{
    public float laserLength = 60f;
    public float initial_delay = 0f;


    void Start()
    {

    }


    void FixedUpdate()
    {
        initial_delay -= Time.deltaTime;
        if (initial_delay <= 0 && !only_shoot_on_command)
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
            bulletSpawned.transform.position = bulletSpawned.transform.position - (transform.up * laserLength);
            bulletSpawned.transform.Rotate(0, 0, -90);
    }
}
