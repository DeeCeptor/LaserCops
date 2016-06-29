using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
    public Vector3 target;
    public float speed = 3f;
    public Vector2 dir;
	public float damage = 15f;
	// Use this for initialization
	void Start () {
        dir = target - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        
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
            // Spawn small sparks and explosion
            EffectsManager.effects.BulletHitPlayer(collision.contacts[0].point);

			collision.gameObject.GetComponent<PlayerController>().TakeHit(damage, true);
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
