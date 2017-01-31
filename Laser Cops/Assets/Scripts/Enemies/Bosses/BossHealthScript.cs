using UnityEngine;
using System.Collections;

public class BossHealthScript : MonoBehaviour {
    public float health = 20f;
    public ConversationManager formChangeConversation;
    public ConversationManager death_conversation;
    //if this field is empty the boss will die when out of health. otherwise the boss will change form when it runs out of health
    public GameObject nextStage;
	//number of stages the boss will have
	public int stages = 3;
	//number of healthpoints remaining for the boss overall(across multiple forms)
	public float overallHealth = 60;
    public string bossName = "Welcome to the GUNSHIP";
    public int pointValue = 2000;
    public int TimeBonus = 600;
    public GameObject ChangeFormEffect;

    public bool explodeOnDeath = false;
    public float explosionCountdownTimer = 0f;
    public float timeBetweenExplosions = 0.2f;

    //time between death and the victory screen
    public float deathDelay = 4f;
    //flag when hp reaches 0
    public bool dying = false;

    protected float tether_lightning_cooldown;

    public bool hurtByTether = false;
    public bool hurtByReboundBullets = false;

    //how much to multiply the damage for on easy and hard mode
    public float easyDamageMultiplyer = 2;
    public float hardDamageMultiplyer = 0.667f;

    //whether the boss should utilize immunity frames
    public bool useImmunityTime = false;
    //how many immunity frames the boss receives (if any)
    public float immunityTime = 0.1f;
    private float immunityCounter = 0f;

    //make a mzimum number of times the tether can hit it in a frame
    public bool hit = false;


    void Start ()
    {
        immunityCounter = Time.time + immunityTime;


        StartCoroutine(ShowHealth());
    }
	

    IEnumerator ShowHealth()
    {
        yield return new WaitForSeconds(7f);

        if (!InGameUIManager.ui_manager.bottom_bar.activeInHierarchy)
        {
            SoundMixer.sound_manager.PlayChargeUp();
            InGameUIManager.ui_manager.ActivateBottomHealthBar(bossName, Color.red, overallHealth);
        }
    }


    void Update()
    {
        tether_lightning_cooldown -= Time.deltaTime;
    }


    void FixedUpdate ()
    {
        hit = false;
	    if(health <= 0 && !dying)
        {
            Die();
        }
        if(dying)
        {
            if(deathDelay < Time.time)
            {
                Explode();
            }
            if (explodeOnDeath)
            {
                DeathEffects();
            }

        }
	}

	public void takeHit(float damage)
	{
		health -= damage;
        overallHealth -= damage;
        InGameUIManager.ui_manager.UpdateBottomHealthBar(overallHealth);
        if (health <= 0)
        {
            Die();
        }
    }

    public void Explode()
    {
        SoundMixer.sound_manager.Play8bitExplosion();
        EffectsManager.effects.ViolentExplosion(this.transform.position);
        //TetherLightning.tether_lightning.BurstLightning((Vector2)this.transform.position + new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)), (Vector2) this.transform.position, 5);
        InGameUIManager.ui_manager.ChangeScore(pointValue, this.transform.position);
        if (Time.timeSinceLevelLoad < TimeBonus)
        {
            InGameUIManager.ui_manager.ChangeScore(TimeBonus - (int)Time.timeSinceLevelLoad, this.transform.position);
        }
        EffectsManager.effects.GridExplosion(this.transform.position, 2f, 8f, Color.red);
        EffectsManager.effects.CutSprite(this.gameObject);

        //GameObject[] corpses = CutSprite();
        //corpses[0].GetComponent<EnemyDying>().JustDied(1);
        //corpses[1].GetComponent<EnemyDying>().JustDied(-1);
        GameState.game_state.Victory();
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 12 && hurtByTether == true && hit == false)
        {
            if (GameState.game_state.current_difficulty == GameState.Difficulty.Hard)
            {
                takeHit(Tether.tether.Damage * hardDamageMultiplyer);
            }
            else if (GameState.game_state.current_difficulty == GameState.Difficulty.Easy)
            {
                takeHit(Tether.tether.Damage * easyDamageMultiplyer);
            }
            else
            {
                takeHit(Tether.tether.Damage);
            }
            hit = true;
            
        }

        if (collision.gameObject.tag == "BounceBomb")
        {
            if(!useImmunityTime)
            {
                if(GameState.game_state.current_difficulty == GameState.Difficulty.Hard)
                {
                    takeHit(collision.gameObject.GetComponent<bounceBomb>().damageToBoss*hardDamageMultiplyer);
                }
                else if (GameState.game_state.current_difficulty == GameState.Difficulty.Easy)
                {
                    takeHit(collision.gameObject.GetComponent<bounceBomb>().damageToBoss * easyDamageMultiplyer);
                }
                else
                {
                    takeHit(collision.gameObject.GetComponent<bounceBomb>().damageToBoss);
                }

                TakeHitGraphics(collision);
            }
            else
            {
                if (immunityCounter < Time.time && useImmunityTime)
                {
                    immunityCounter = Time.time + immunityTime;
                    if (GameState.game_state.current_difficulty == GameState.Difficulty.Hard)
                    {
                        takeHit(collision.gameObject.GetComponent<bounceBomb>().damageToBoss * hardDamageMultiplyer);
                    }
                    else if (GameState.game_state.current_difficulty == GameState.Difficulty.Easy)
                    {
                        takeHit(collision.gameObject.GetComponent<bounceBomb>().damageToBoss * easyDamageMultiplyer);
                    }
                    else
                    {
                        takeHit(collision.gameObject.GetComponent<bounceBomb>().damageToBoss);
                    }

                    TakeHitGraphics(collision);
                }
            }
            Destroy(collision.gameObject);
        }
    }

    public void TakeHitGraphics(Collision2D collision)
    {
        EffectsManager.effects.ViolentExplosion(collision.contacts[0].point);
        EffectsManager.effects.TetherGrindSparks(collision.contacts[0].point);
        EffectsManager.effects.BurstLargeFireball(collision.contacts[0].point);
        SoundMixer.sound_manager.PlayGettingHitExplosion();
    }

    //Changes forms or dies depending on whether this is the final form
    public void Die()
    {
        if(nextStage!=null)
        {
            Destroy(gameObject);
            PlayConversation(formChangeConversation);
            if(ChangeFormEffect!= null)
            {
                Instantiate(ChangeFormEffect, transform.position, transform.rotation);
            }
            Instantiate(nextStage,transform.position,transform.rotation);
        }
        else
        {
            if (death_conversation != null)
            {
                PlayConversation(death_conversation);
            }
            deathDelay = Time.time + deathDelay;
            dying = true;
        }
    }

    public void HitByTetherGraphics(Collision2D collision)
    {
        SoundMixer.sound_manager.PlaySyncopatedLazer();

        if (tether_lightning_cooldown <= 0)
        {
            tether_lightning_cooldown = 0.1f;
            //EffectsManager.effects.TetherDamageSparks(collision.contacts[0].point);
            TetherLightning.tether_lightning.BranchLightning(Tether.tether.GetRandomLink().transform.position, this.transform.position);
        }
    }

    //adds dialogue to the screen for changing forms or death
    public void PlayConversation(ConversationManager conversation)
    {
        if (conversation != null)
        {
            conversation.transform.SetParent(null);
            conversation.Start_Conversation();
        }
    }

    public void DeathEffects()
    {
        if(explosionCountdownTimer < Time.time)
        {
            Vector2 Rando = new Vector2(Random.Range(-5,5), Random.Range(-10, 10));
            SoundMixer.sound_manager.Play8bitExplosion();
            EffectsManager.effects.BurstLargeFireball((Vector2)transform.position + Rando);
            explosionCountdownTimer = Time.time + timeBetweenExplosions;
        }
    }
}
