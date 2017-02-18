using UnityEngine;
using System.Collections;

//this class shoots a laser using a raycast and a line renderer that will shoot from the object until it hits a collider or reaches max laser length, damages player on contact
public class RayLaserScript : MonoBehaviour
{
    //whether the laser sound has started;
    public bool soundStarted = false;
    //how long between shots
    public float shotDelay = 5f;
    //how long the shot lasts
    public int shotDuration = 150;
    //gives the player a time at the start of the shot where they see the laser but it cannot harm them
    public int immuneFrames = 50;
    //counter for immunitytime and shot duration
    public int TimeSinceShotCounter = 0;
    //counter for how long between shots
    public float shotCounter = 0f;
    public LineRenderer laserRenderer;
    public bool active = false;
    private RaycastHit2D hit;
    public float maxLaserLength = 100f;
    public bool shooting = false;
    //important note this is damage per "tick"
    public float damage = 0.5f;
    //layers to ignore for the raycast
    public LayerMask layersToIgnore;
    //whether to play sound
    public bool silent = false;
    public _Colour bullet_colour = _Colour.Red;
    //set to true to alternate between pink and blue each shot
    public bool alternateBulletColour = false;
    //set to true to randomely pick pink or blue when the object starts
    public bool randomPinkOrBlue = false;

    //true if you want it to disable itself when the player is close
    public bool playerCloseDisable = true;
    //how close the player must be to disable the shot
    public float disableDistance = 2f;
    public bool DelayBeforeFire = false;

    [HideInInspector]
    public Material cyan_glow;
    [HideInInspector]
    public Material pink_glow;
    [HideInInspector]
    public Material red_glow;

    void Start ()
    {
        pink_glow = (Material)Resources.Load("Materials/StreakGlowPink");
        cyan_glow = (Material)Resources.Load("Materials/StreakGlowCyan");
        red_glow = (Material)Resources.Load("Materials/StreakGlowRed");

        if (randomPinkOrBlue)
        {
            if(Random.Range(0,2) > 0)
            {
                bullet_colour = _Colour.Blue;
                laserRenderer.SetColors(Color.cyan, Color.cyan);
            }
            else
            {
                bullet_colour = _Colour.Pink;
                laserRenderer.SetColors(Color.magenta, Color.magenta);
            }
        }
        layersToIgnore = ~((1<<12)|(1<<13) | (1 << 15)|(1<<0)| (1 << 22) | (1 << 23) | (1 << 24)| (1 << 26) | (1 << 8));
        if(DelayBeforeFire)
        {
            shotCounter = Time.time + shotDelay;
        }
	}
	

	void FixedUpdate ()
    {
        if (active)
        {
            if (shotCounter < Time.time)
            {
                if (!silent)
                {
                    SoundMixer.sound_manager.PlayChargeUp();
                }
                laserRenderer.SetWidth(0.5f, 0.5f);
                shooting = true;
                TimeSinceShotCounter = 0;
                laserRenderer.enabled = true;
                shotCounter = Time.time + shotDelay;

                switch (bullet_colour)
                {
                    case _Colour.Red:
                        laserRenderer.material = red_glow;
                        break;
                    case _Colour.Blue:
                        laserRenderer.material = cyan_glow;
                        break;
                    case _Colour.Pink:
                        laserRenderer.material = pink_glow;
                        break;
                }
            }

            if (shooting)
            {
                Shoot();

                if (TimeSinceShotCounter > shotDuration)
                {
                    SoundMixer.sound_manager.StopBigLazerSound();
                    soundStarted = false;
                    shooting = false;
                    laserRenderer.enabled = false;
                    if (alternateBulletColour)
                    {
                        if (bullet_colour == _Colour.Pink)
                        {
                            bullet_colour = _Colour.Blue;
                            laserRenderer.SetColors(Color.cyan, Color.cyan);
                        }
                        else
                        {
                            bullet_colour = _Colour.Pink;
                            laserRenderer.SetColors(Color.magenta, Color.magenta);
                        }
                    }
                }
            }
        }
	}


    public void Shoot()
    {
        //see if there is an obstacle in the way
        hit = Physics2D.Raycast(transform.position, transform.up,float.PositiveInfinity,layersToIgnore);

        //if there is an obstacle then 
        if (hit.collider != null)
        {
            laserRenderer.SetPosition(0, transform.position);
            laserRenderer.SetPosition(1, hit.point);
        }
        else
        {
            laserRenderer.SetPosition(0, transform.position);
            laserRenderer.SetPosition(1, transform.position + (transform.up * maxLaserLength));
        }

        //if the laser is supposed to do damage
        if (TimeSinceShotCounter >= immuneFrames)
        {
            if (!silent && soundStarted == false)
            {
                SoundMixer.sound_manager.StopChargeUp();
                SoundMixer.sound_manager.PlayBigLazerSound();
                
                soundStarted = true;
            }
            laserRenderer.SetWidth(2,1);
            if(hit.collider!=null)
            {
                if(hit.collider.gameObject.CompareTag("Player"))
                {
                    if(bullet_colour != hit.collider.gameObject.GetComponent<PlayerController>().player_colour)
                    {
                        hit.collider.gameObject.GetComponent<PlayerController>().TakeHit(damage, true);
                    }

                }
            } 
        }

        TimeSinceShotCounter += 1;
    }
}
