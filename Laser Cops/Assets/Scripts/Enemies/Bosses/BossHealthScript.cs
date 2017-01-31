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
	    if(health <= 0)
        {
            Die();
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

    //cuts sprite as a death effect
    public GameObject[] CutSprite()
    {
        Sprite corpseSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        Texture2D[] corpses = new Texture2D[2];
        Texture2D tex;
        GameObject[] corpse_objects = new GameObject[2];

        //instantiate the new sprites for cutting
        for (int i = 0; i < 2; i++)
        {
            GameObject corpseSpawned = (GameObject)Instantiate(Resources.Load("enemies/EmptyCorpse"), transform.position, transform.rotation);
            corpse_objects[i] = corpseSpawned;
            corpseSpawned.transform.localScale = transform.localScale;

            tex = gameObject.GetComponent<SpriteRenderer>().sprite.texture;
            corpses[i] = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
            corpseSpawned.GetComponent<SpriteRenderer>().sprite = Sprite.Create(corpses[i], corpseSprite.rect, new Vector2(0.5f, 0.5f));
            corpseSpawned.GetComponent<SpriteRenderer>().color = gameObject.GetComponent<SpriteRenderer>().color;
        }

        //get a random point along a circles edge from the transforms position then get an opposite point 
        Vector2 rand = Random.insideUnitCircle;
        Vector2 point1 = transform.position + (Vector3)rand;
        Vector2 point2 = transform.position - (Vector3)rand;

        Vector2[] pixelLocations = new Vector2[corpses[0].GetPixels32().Length];
        float width = corpses[0].width;

        //random varience to make it look like it was sheared
        //maximum varience
        float xVariance = Random.Range(3f, 5f);
        //current varience
        float xToVary = xVariance;
        //whether x will iterate up or down 
        bool xUp = false;
        float yVariance = Random.Range(3f, 5f);
        float yToVary = yVariance;
        bool yUp = false;
        //counter for when to change x
        int currentIterations = 0;

        int iterationsNeeded = (int)width;
        //set worldspace locations for the pixels
        for (int i = 0; i < pixelLocations.Length; i++)
        {
            pixelLocations[i] = new Vector2((transform.position.x - width / 2f) + (1f * (i % (int)width)) + xToVary, (transform.position.y - width / 2f) + (1f * (i / (int)width)) + yToVary);
            //scripts underneath are to make it jagged
            if (yUp)
            {
                if (yToVary >= yVariance)
                {
                    yUp = false;
                    yToVary = yToVary - 1;
                }
                else
                {
                    yToVary = yToVary + 1;
                }
            }
            else
            {
                if (yToVary <= -yVariance)
                {
                    yUp = true;
                    yToVary = yToVary + 1;
                }
                else
                {
                    yToVary = yToVary - 1;
                }
            }

            if (currentIterations > iterationsNeeded)
            {
                currentIterations = 0;
                if (xUp)
                {
                    if (xToVary >= xVariance)
                    {
                        xUp = false;
                        xToVary = xToVary - 1;
                    }
                    else
                    {
                        xToVary = xToVary + 1;
                    }
                }
                else
                {
                    if (xToVary <= -xVariance)
                    {
                        xUp = true;
                        xToVary = xToVary + 1;
                    }
                    else
                    {
                        xToVary = xToVary - 1;
                    }
                }
            }
            currentIterations = currentIterations + 1;
        }

        //array of pixel arrays
        Color32[][] vertices = new Color32[2][];

        for (int i = 0; i < 2; i++)
        {
            vertices[i] = corpseSprite.texture.GetPixels32();
        }

        for (int i = 0; i < pixelLocations.Length; i++)
        {
            //see which side the point will be on
            if ((pixelLocations[i] - point1).magnitude < (pixelLocations[i] - point2).magnitude)
            {
                //set pixel to clear for the side it's not on
                vertices[1][i] = Color.clear;

            }
            else
            {
                vertices[0][i] = Color.clear;
            }
        }

        for (int i = 0; i < 2; i++)
        {

            corpses[i].SetPixels32(vertices[i]);
            corpses[i].Apply(true);

        }

        return corpse_objects;
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

        //GameObject[] corpses = CutSprite();
        //corpses[0].GetComponent<EnemyDying>().JustDied(1);
        //corpses[1].GetComponent<EnemyDying>().JustDied(-1);

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
        Destroy(gameObject);
        if(nextStage!=null)
        {
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
            Explode();
            GameState.game_state.Victory();
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
}
