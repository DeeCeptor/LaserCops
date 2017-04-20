using UnityEngine;
using System.Collections.Generic;

public class ForwardShotScript : MonoBehaviour
{
    //will shoot forward at regular intervals
    public float shotDelay = 0.8f;
    public float shotCounter;
    public GameObject bullet;
    public bool randomly_choose_coloured_bullet = false;
    public List<GameObject> coloured_bullets;
    [HideInInspector]
    public bool active = false;
    public bool only_shoot_on_command = false;  // Will only fire when shoot() is called

    float shot_timer = 0;

    public bool randomly_determine_colour = false;
    

    void Start ()
    {
	    if (randomly_choose_coloured_bullet && coloured_bullets.Count > 0)
        {
            int range = Mathf.Min(coloured_bullets.Count, GameState.game_state.number_of_players);
            bullet = coloured_bullets[Random.Range(0, range)];
        }
	}


    void FixedUpdate()
    {
        if (!only_shoot_on_command)
        {
            if (active)
            {
                shot_timer -= Time.fixedDeltaTime * Time.timeScale;
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
    }

    public void shoot()
    {
        SoundMixer.sound_manager.PlayLazerShot();
        CreateBullet();
    }


    public void CreateBullet()
    {
        GameObject bulletSpawned = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
        BulletScript bulletStats = bulletSpawned.GetComponent<BulletScript>();
        bulletStats.target = transform.position + transform.up;
        
    }
}
