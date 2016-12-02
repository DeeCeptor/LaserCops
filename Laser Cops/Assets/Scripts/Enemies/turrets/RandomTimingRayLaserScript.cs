using UnityEngine;
using System.Collections;

public class RandomTimingRayLaserScript : RayLaserScript {
    public float maxShotDelay = 8f;
    public float minShotDelay = 4f;
    

	// Use this for initialization
	void Start () {
        shotCounter = Random.Range(minShotDelay,maxShotDelay) + Time.time;
        layersToIgnore = ~((1 << 12) | (1 << 13) | (1 << 15) | (1 << 0) | (1 << 22) | (1 << 23) | (1 << 24) | (1 << 26) | (1 << 8));
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (shotCounter < Time.time)
        {
            laserRenderer.SetWidth(0.5f, 0.5f);
            shooting = true;
            TimeSinceShotCounter = 0;
            laserRenderer.enabled = true;
            shotCounter = Time.time + shotDelay;
        }

        if (shooting)
        {
            if (TimeSinceShotCounter > shotDuration)
            {
                shooting = false;
                laserRenderer.enabled = false;
            }
            Shoot();
            shotCounter = Random.Range(minShotDelay, maxShotDelay) + Time.time;
        }
    }
}
