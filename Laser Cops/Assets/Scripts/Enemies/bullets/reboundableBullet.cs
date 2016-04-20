using UnityEngine;
using System.Collections;

public class reboundableBullet : BulletScript
{
    public float damageToBoss = 2f;
    public Color reboundColor = Color.blue;
    public bool rebounding = false;
    public int reboundLayer = 12;
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Spawn small sparks and explosion
            EffectsManager.effects.BulletHitPlayer(collision.contacts[0].point);

            collision.gameObject.GetComponent<PlayerController>().TakeHit(damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "VIP")
        {
            collision.gameObject.GetComponent<VIPScript>().TakeHit(damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Tether")
        {
            gameObject.tag = "ReboundingBullet";
            rebounding = true;
            dir =  (Vector2) transform.position - (Vector2)collision.contacts[0].point;
            gameObject.layer = reboundLayer;
            GetComponent<SpriteRenderer>().color = reboundColor;
        }

        else if(collision.gameObject.tag == "Enemy" && rebounding)
        {
            basicScrollingEnemyScript enemyHealth = collision.gameObject.GetComponent<basicScrollingEnemyScript>();
            enemyHealth.Die();
            Destroy(gameObject);

        }

        else if(collision.gameObject.tag == "Boss" && rebounding)
        {
            BossHealthScript bossHealth = collision.gameObject.GetComponent<BossHealthScript>();
            bossHealth.takeHit(damageToBoss);
            EffectsManager.effects.ViolentExplosion(this.transform.position);
            Destroy(gameObject);
        }


    }
}
