using UnityEngine;
using System.Collections;

public enum _Colour { Red, Yellow, Pink, Blue };

public class BulletScript : MonoBehaviour
{
    public Vector3 target;
    public float speed = 3f;
    public Vector2 dir;
	public float damage = 15f;

    public _Colour bullet_colour = _Colour.Red;

    void Start ()
    {
        dir = target - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }


    void FixedUpdate ()
    {
        GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
        CheckDeath();
    }

    public void CheckDeath()
    {
        if(!GetComponent<SpriteRenderer>().isVisible)
        {
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
		if(collision.gameObject.tag == "Player")
		{
            if (collision.gameObject.GetComponent<PlayerController>().player_colour == bullet_colour)
            {
                Die();
                EffectsManager.effects.SameColorHit(collision.contacts[0].point, bullet_colour);
            }
            else
            {
                collision.gameObject.GetComponent<PlayerController>().TakeHit(damage, true);
                EffectsManager.effects.BulletHitPlayer(collision.contacts[0].point);
                
            }

            // Spawn small sparks and explosion
            
		}

		if (collision.gameObject.tag == "VIP")
		{
			collision.gameObject.GetComponent<VIPScript>().TakeHit( damage);
		}

		if (!collision.gameObject.CompareTag("Enemy"))
		{
			Die();
		}
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
