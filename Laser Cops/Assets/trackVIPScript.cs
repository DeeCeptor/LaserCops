using UnityEngine;
using System.Collections;

public class trackVIPScript : TrackShotScrolling {
    public Transform VIP;
	// Use this for initialization
	void Start () {
        VIP = GameState.game_state.VIPObject.transform;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
	    if(VIP == null)
        {
            VIP = GameState.game_state.VIPObject.transform;
        }
        if (active)
        {
            if (shotCounter < Time.time)
            {
                shotCounter = Time.time + shotDelay;

                if (VIP != null)
                {
                    Shoot();
                }
            }
        }
        else
        {
            checkActive();
        }
    }

    public void Shoot()
    {
            GameObject bulletSpawned = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
            BulletScript bulletStats = bulletSpawned.GetComponent<BulletScript>();
            bulletStats.target = VIP.position;
    }
}
