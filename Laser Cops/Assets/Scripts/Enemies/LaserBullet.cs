using UnityEngine;
using System.Collections;

public class LaserBullet : BulletScript
{
    public float sparkCooldown = 1f;
    public float sparkCounter;
	// Use this for initialization
	void Start () {
        sparkCounter = 0f;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (sparkCounter < Time.time)
            {
                // Spawn small sparks and explosion
                EffectsManager.effects.BulletHitPlayer(collision.transform.position);
                sparkCounter = sparkCooldown + Time.time;
            }

            collision.gameObject.GetComponent<PlayerController>().TakeHit(damage, true);
        }

        if (collision.gameObject.tag == "VIP")
        {
            collision.gameObject.GetComponent<VIPScript>().TakeHit(damage);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (sparkCounter < Time.time)
            {
                // Spawn small sparks and explosion
                EffectsManager.effects.BulletHitPlayer(collision.transform.position);
                sparkCounter = sparkCooldown + Time.time;
            }

            if (collision.gameObject.GetComponent<PlayerController>().player_colour == bullet_colour)
            {
                //could do effect here but it might occur too many times and lead to a slowdown
            }
            else
            {
                collision.gameObject.GetComponent<PlayerController>().TakeHit(damage, true);
            }
                
        }

        if (collision.gameObject.tag == "VIP")
        {
            collision.gameObject.GetComponent<VIPScript>().TakeHit(damage);
        }
    }
}
