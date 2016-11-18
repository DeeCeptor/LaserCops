using UnityEngine;
using System.Collections;


public class ShootLaserShootingBullets : ForwardShotScript {
    //this script is for use in the Disco Dan bossfight using it elsewhere might be unpredictable
    //blue bullet
    public GameObject blueGun;
    //pinkbullet
    public GameObject pinkGun;
    public bool top;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (shotCounter < Time.time)
        {
            shotCounter = Time.time + shotDelay;
            shoot();
        }
    }

    public new void shoot()
    {
        //if it's the top turret it needs to be spawned upside down to create a laser gate effect
        if (top)
        {
            GameObject bulletSpawned = (GameObject)Instantiate(pinkGun, transform.position, new Quaternion(0,0,90,0));
            SoundMixer.sound_manager.PlayLazerShot();
        }
        else
        {
            GameObject bulletSpawned = (GameObject)Instantiate(blueGun, transform.position, new Quaternion(0, 90, 0, 0));
            SoundMixer.sound_manager.PlayLazerShot();
        }
          

     }
}
