using UnityEngine;
using System.Collections;

public enum _Colour { Red, Pink, Blue };

public class BulletScript : MonoBehaviour
{
    public Vector3 target;
    public float speed = 3f;
    public float originalSpeed = 3f;
    public Vector2 dir;
	public float damage = 15f;

    public bool reflected_bullet = false;   // It will hit enemies
    public _Colour bullet_colour = _Colour.Red;

    void Start ()
    {
        originalSpeed = speed;
        dir = target - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
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
                SoundMixer.sound_manager.PlayShortSpark();
            }
            else
            {
                collision.gameObject.GetComponent<PlayerController>().TakeHit(damage, true);
                EffectsManager.effects.BulletHitPlayer(collision.contacts[0].point);
                Die();
            }

            // Spawn small sparks and explosion
            
		}

		else if (collision.gameObject.tag == "VIP")
		{
			collision.gameObject.GetComponent<VIPScript>().TakeHit( damage);
            Die();
        }

        else if (reflected_bullet && collision.gameObject.CompareTag("Boss"))
        {
            BossHealthScript bs = collision.gameObject.GetComponent<BossHealthScript>();
            if (bs.hurtByReboundBullets)
            {
                bs.takeHit(damage);
                bs.TakeHitGraphics(collision);
            }

            Die();
        }


        else if (reflected_bullet && collision.gameObject.CompareTag("Enemy"))
        {
            // Deal damage to the reflect enemy
            basicScrollingEnemyScript sc = collision.gameObject.GetComponent<basicScrollingEnemyScript>();
            if (sc != null)
            {
                sc.TakeHit(damage);
                EffectsManager.effects.BulletHitPlayer(collision.contacts[0].point);
                Die();
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("CaptureTether"))
        {
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("EnemyBossTether") && reflected_bullet)
		{
            speed = originalSpeed;
            reflected_bullet = !reflected_bullet;
            gameObject.layer = LayerMask.NameToLayer("Bullet");
            SoundMixer.sound_manager.PlayShortSpark();

            // Find out which colour of bullet we were before
            switch (bullet_colour)
            {
                case _Colour.Red:
                    GetComponent<SpriteRenderer>().color = Color.red;
                    EffectsManager.effects.PlayerBulletReflected(this.transform.position);
                    break;
                case _Colour.Pink:
                    GetComponent<SpriteRenderer>().color = Color.magenta;
                    EffectsManager.effects.SameColorHit(this.transform.position, bullet_colour);
                    break;
                case _Colour.Blue:
                    GetComponent<SpriteRenderer>().color = Color.cyan;
                    EffectsManager.effects.SameColorHit(this.transform.position, bullet_colour);
                    break;
            }
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
