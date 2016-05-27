using UnityEngine;
using System.Collections;

//this script was copied from the laser spawn script so some variable names may be confusing 
//this script does the same thing as the laser spawn script except it does not set the spawned object to be it's child
public class pillarSpawnerScript : ForwardShotScript
{
    public float laserLength = 60f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
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

        if (!playerCloseDisable)
        {
            GameObject bulletSpawned = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
            bulletSpawned.transform.position = bulletSpawned.transform.position - (transform.up * laserLength);
        }

        else
        {
            bool fire = true;
            for (int i = 0; i < GameState.game_state.Players.Count; i++)
            {
                if ((GameState.game_state.Players[i].transform.position - transform.position).magnitude < disableDistance)
                {
                    fire = false;
                }
            }
            if (fire)
            {
                GameObject bulletSpawned = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
                bulletSpawned.transform.position = bulletSpawned.transform.position - (transform.up * laserLength);
            }
        }
    }
}
