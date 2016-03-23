using UnityEngine;
using System.Collections;

public class ForwardShotScript : MonoBehaviour {
    //will shoot forward at regular intervals
    public float shotDelay = 0.5f;
    public float shotCounter;
    public GameObject bullet;
    public bool active = false;

    //true if you want it to disable itself when the player is close
    public bool playerCloseDisable = true;
    //how close the player must be to disable the shot
    public float disableDistance = 2f;

        

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
        if (!playerCloseDisable)
        {
            GameObject bulletSpawned = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
            BulletScript bulletStats = bulletSpawned.GetComponent<BulletScript>();
            bulletStats.target = transform.position - transform.up;
        }

        else
        {
            bool fire = true;
			for(int i = 0; i < GameState.game_state.Players.Count; i++)
            {
				if ((GameState.game_state.Players[i].transform.position -transform.position).magnitude < disableDistance)
                {
                    fire = false;
                }
            }
            if(fire)
            {
                GameObject bulletSpawned = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
                BulletScript bulletStats = bulletSpawned.GetComponent<BulletScript>();
                bulletStats.target = transform.position - transform.up;
            }
        }
    }
}
