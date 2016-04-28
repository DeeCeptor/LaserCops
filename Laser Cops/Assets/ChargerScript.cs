using UnityEngine;
using System.Collections;

public class ChargerScript : scrollThenChase {
    public float chargeCooldown = 5f;
    public float chargeTimer = 0f;
    public float chargeDuration = 3f;
    public float chargeSpeed = 7f;
    public Vector3 chargeDir;
    public bool Charging = false;


	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        tether_lightning_cooldown -= Time.deltaTime;

        if (!active)
        {
            CheckActive();
            moveInactive();
        }
        else
        {
            CheckDeath();

            if (!stopped)
            {
                moveActive();
                if (stopCounter < Time.time)
                {
                    stopped = true;
                    speed = speedAfterStop;
                    chargeTimer = Time.time + chargeCooldown;
                }
            }
            else
            {
                if (!Charging)
                {
                    if (chargeTimer < Time.time)
                    {
                        Charging = true;
                        chargeDir = playerToTrack.position - transform.position;
                    }
                    Chase();
                }
                else if (Charging)
                {
                    Charge();
                    if (chargeTimer < Time.time - chargeDuration)
                    {
                        Charging = false;
                        chargeTimer = Time.time + chargeCooldown;
                    }
                }

            }
        }
    }

    public void Charge()
    {
        desired_velocity = chargeDir.normalized * chargeSpeed;
        GetComponent<Rigidbody2D>().velocity = desired_velocity;
    }
}
