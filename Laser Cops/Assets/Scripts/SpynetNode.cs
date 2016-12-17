using UnityEngine;
using System.Collections;

public class SpynetNode : basicScrollingEnemyScript {

    //when this object dies it is supposed to deal damage to a boss
    public float damageToBoss = 10f;
	// Use this for initialization
	void Start () {
        initiate();
    }
	
	// Update is called once per frame
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
