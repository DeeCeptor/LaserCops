using UnityEngine;
using System.Collections;

//class for which direction the enemy will be travelling in
public enum direction
{
    up = 0, left = 1, down = 2, right = 3
};

//this is a template for scrolling enemies and simply falls but has methods for doing more complex things
public class basicScrollingEnemyScript : MonoBehaviour
{
    public float speed = 2f;
    public bool active = false;
    public bool die_in_one_hit = false;
    public bool takes_grinding_damage = false;
	public float collisionDamage = 0.3f;
	public int pointValue = 100;
	public float health = 1f;
    public bool beingRammed = false;

    [HideInInspector]
    public Vector2 desired_velocity = Vector2.zero;
    
    //this is used for the enemies speed when OFFSCREEN do not change unless you know what you're doing in which case I'm a comment not a cop
    //public float GameState.game_state.inactive_speed = 1f;

    //direction the enemy will travel towards
    public direction travelDirection = direction.left;

    protected float tether_lightning_cooldown;

    // Use this for initialization
    void Start()
    {
        initiate();
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
            moveActive();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        ResolveCollision(collision);
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        ResolveCollision(collision);
    }
    void ResolveCollision(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("DestructiveTether"))
        {
            HitByTetherGraphics(collision);

            if (die_in_one_hit)
                Die();
            else
                TakeHit(Tether.tether.Damage);
        }

        else if (collision.gameObject.tag == "Player")
        {
            // Hurt the player
            collision.gameObject.GetComponent<PlayerController>().TakeHit(collisionDamage);
            beingRammed = true;

            if (takes_grinding_damage)
            {
                // Hurt this enemy cause it's grinding against the player
                this.TakeHit(collision.gameObject.GetComponent<PlayerController>().Grinding_Damage);
            }
        }

        else if (collision.gameObject.tag == "VIP")
        {
            collision.gameObject.GetComponent<VIPScript>().TakeHit(collisionDamage);
        }

        else if (collision.gameObject.layer == LayerMask.NameToLayer("RamBoundary"))
        {
            // Hurt the enemy
            TakeHit(collision.gameObject.GetComponent<HurtPlayerOnTouch>().collisionDamage*5);
        }

        else if (collision.gameObject.tag == "Obstacle")
        {
            if (beingRammed)
            {
                // Hurt the enemy
                TakeHit(collision.gameObject.GetComponent<ObstacleScrollScript>().damage * 5);
            }
            if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude >= 10)
            {
                Die();
            }
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

    public void moveInactive()
    {
        if(travelDirection == direction.left)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-GameState.game_state.inactive_speed,0);
        }
        else if (travelDirection == direction.up)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GameState.game_state.inactive_speed);
        }
        else if (travelDirection == direction.right)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GameState.game_state.inactive_speed, 0);
        }
        else if (travelDirection == direction.down)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -GameState.game_state.inactive_speed);
        }
    }

    public void moveActive()
    {
        if (travelDirection == direction.left)
        {
            desired_velocity = new Vector2(-speed, 0);
            GetComponent<Rigidbody2D>().velocity = desired_velocity;
        }
        else if (travelDirection == direction.up)
        {
            desired_velocity = new Vector2(0, speed);
            GetComponent<Rigidbody2D>().velocity = desired_velocity;
        }
        else if (travelDirection == direction.right)
        {
            desired_velocity = new Vector2(speed, 0);
            GetComponent<Rigidbody2D>().velocity = desired_velocity;
        }
        else if (travelDirection == direction.down)
        {
            desired_velocity = new Vector2(0, -speed);
            GetComponent<Rigidbody2D>().velocity = desired_velocity;
        }
    }

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

        Texture2D sprite = corpses[0];
        //array of pixel arrays
        Color32[][] vertices = new Color32[2][];

        for (int i = 0; i < 2; i++)
        {
            vertices[i] = corpseSprite.texture.GetPixels32();
        }

        for (int i = 0; i < pixelLocations.Length; i++)
        {
            //see which side the point will be on
            if((pixelLocations[i] - point1).magnitude < (pixelLocations[i] - point2).magnitude)
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

    public void Die()
    {
        SoundMixer.sound_manager.Play8bitExplosion();
        EffectsManager.effects.ViolentExplosion(this.transform.position);
        //TetherLightning.tether_lightning.BurstLightning((Vector2)this.transform.position + new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)), (Vector2) this.transform.position, 5);
        UIManager.ui_manager.ChangeScore(pointValue, this.transform.position);
        EffectsManager.effects.GridExplosion(this.transform.position, 2f, 8f, Color.red);

        GameObject[] corpses = CutSprite();
        corpses[0].GetComponent<EnemyDying>().JustDied(1);
        corpses[1].GetComponent<EnemyDying>().JustDied(-1);

        Destroy(gameObject);
    }

	//to be used whenthe enemy dies offscreen
	public void DieOffScreen()
	{
		Destroy(gameObject);
	}

    public void initiate()
    {

    }

    //check if enemy is on screen. Scrolling enemies should not shoot offscreen
    public bool isActive()
    {
        if (GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //check if enemy has entered the screen and must activate
    public void CheckActive()
    {
        if (GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
        {
            Activate();
        }
    }

    //put any behaviour that must be done when the enemy enters the screen here
    public void Activate()
    {
        active = true;
    }

    //after activating the enemy should die if it leaves the screen
    public void CheckDeath()
    {
        if (!GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
        {
            DieOffScreen();
        }

		if(health<=0)
		{
			Die();
		}
    }

	public void TakeHit(float damage)
	{
		health -= damage;
	}
}

