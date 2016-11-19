using UnityEngine;
using System.Collections;

public class ForwardShotScript : MonoBehaviour
{
    //will shoot forward at regular intervals
    public float shotDelay = 0.5f;
    public float shotCounter;
    public GameObject bullet;
    public bool active = false;
    public bool only_shoot_on_command = false;  // Will only fire when shoot() is called

    //true if you want it to disable itself when the player is close
    public bool playerCloseDisable = true;
    //how close the player must be to disable the shot
    public float disableDistance = 2f;

    float shot_timer = 0;

    public bool randomly_determine_colour = false;
    

    void Start ()
    {
	
	}


    void FixedUpdate()
    {
        if (!only_shoot_on_command)
        {
            if (active)
            {
                shot_timer -= Time.fixedDeltaTime;
                if (shot_timer <= 0)
                {
                    shot_timer = shotDelay;
                    shoot();
                }
            }
            else
            {
                checkActive();
            }
        }
    }


    public void checkActive()
    {
        if (GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
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
            CreateBullet();
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
            if (fire)
            {
                CreateBullet();
            }
        }
    }


    public void CreateBullet()
    {
        GameObject bulletSpawned = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
        BulletScript bulletStats = bulletSpawned.GetComponent<BulletScript>();
        bulletStats.target = transform.position + transform.up;
        SoundMixer.sound_manager.PlayLazerShot();
    }
}
