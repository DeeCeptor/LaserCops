using UnityEngine;
using System.Collections;

public class SpynetNode : basicScrollingEnemyScript
{

    //when this object dies it is supposed to deal damage to a boss
    public float damageToBoss = 10f;

    void Start ()
    {
        initiate();
    }


    void Update () {
        if (!active)
        {
            moveInactive();
        }
        else
        {
            CheckDeathSpy();
            moveActive();

        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("DestructiveTether"))
        {
            HitByTetherGraphics(collision);

            if (die_in_one_hit)
                Die();
            else
                TakeHit(Tether.tether.Damage);

            return;
        }

        else if (collision.gameObject.CompareTag("Player"))
        {
            HitByTetherGraphics(collision);
                TakeHit(Tether.tether.Damage);
            collision.gameObject.GetComponent<PlayerController>().TakeHit(collisionDamage);

            return;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("DestructiveTether"))
        {
            HitByTetherGraphics(collision);

            if (die_in_one_hit)
                Die();
            else
                TakeHit(Tether.tether.Damage);

            return;
        }


        else if (collision.gameObject.CompareTag("Player"))
        {
            HitByTetherGraphics(collision);
            TakeHit(Tether.tether.Damage);
            collision.gameObject.GetComponent<PlayerController>().TakeHit(collisionDamage);

            return;
        }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("RamBoundary"))
        {
            // Hurt the enemy
            TakeHit(collision.gameObject.GetComponent<HurtPlayerOnTouch>().collisionDamage * 5);
        }
    }


    public void CheckDeathSpy()
    {
        if (health <= 0)
        {
            BossHealthScript bossHealth = GameObject.Find("SpyNet").GetComponent<BossHealthScript>();
            bossHealth.takeHit(damageToBoss);
            Die();
        }
    }
}
