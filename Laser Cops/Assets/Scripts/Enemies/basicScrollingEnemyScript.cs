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

    //whether this enemy is colliding with another enemy
    public bool enemyEnemyCollsion = false;
    //which direction to go when hitting another enemy, this is determined upon the collision and just determines whether a negative multiplyer will be added to the perpendicular velocity
    public bool ramNavPos = false;
    //max time to spend avoiding a collision
    public float collisionAvoidTime = 1f;
    public float collisionAvoidCounter = 0f;

    [HideInInspector]
    public Vector2 desired_velocity = Vector2.zero;
    
    //this is used for the enemies speed when OFFSCREEN do not change unless you know what you're doing in which case I'm a comment not a cop
    //public float GameState.game_state.inactive_speed = 1f;

    //direction the enemy will travel towards
    public direction travelDirection = direction.left;

    protected float tether_lightning_cooldown;
    float tether_lightning_delay = 0.2f;

    public bool navigate_around_enemies = true;

    ManualTrail[] objects_to_activate_when_active;


    void Start()
    {
        initiate();

        // Turn off trails
        objects_to_activate_when_active = this.GetComponentsInChildren<ManualTrail>(true);
        foreach (ManualTrail trail in objects_to_activate_when_active)
        {
            trail.gameObject.SetActive(false);
        }
    }


    void FixedUpdate()
    {
        if (!active)
        {
            moveInactive();
        }
        else
        {
            CheckDeath();
            moveActive();
            
        }
    }

    void Update()
    {
        tether_lightning_cooldown -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("MainCamera"))
        {
            Activate();
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (!active)
        {
            if (collider.gameObject.tag.Equals("MainCamera"))
            {
                Activate();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "MainCamera")
        {
            DieOffScreen();
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

            return;
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

            return;
        }
        else if (collision.gameObject.tag == "VIP")
        {
            collision.gameObject.GetComponent<VIPScript>().TakeHit(collisionDamage);

            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("RamBoundary"))
        {
            // Hurt the enemy
            TakeHit(collision.gameObject.GetComponent<HurtPlayerOnTouch>().collisionDamage*5);
        }

        if (collision.gameObject.tag == "Obstacle")
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
        else if(collision.gameObject.tag == "Enemy")
        {
            if(!enemyEnemyCollsion && collision.gameObject.GetComponent<basicScrollingEnemyScript>()!=null)
            {
                enemyEnemyCollsion = true;
                collisionAvoidCounter = Time.time + collisionAvoidTime;
                int rand = Random.Range(0,1);
                if(rand == 1)
                {
                    ramNavPos = true;
                    collision.gameObject.GetComponent<basicScrollingEnemyScript>().ramNavPos = false;
                }
                else
                {
                    ramNavPos = false;
                    collision.gameObject.GetComponent<basicScrollingEnemyScript>().ramNavPos = true;
                }
            }
        }
    }

    public void HitByTetherGraphics(Collision2D collision)
    {
        SoundMixer.sound_manager.PlaySyncopatedLazer();

        if (tether_lightning_cooldown <= 0)
        {
            tether_lightning_cooldown = tether_lightning_delay;
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
        if (enemyEnemyCollsion)
        {
            navigateAround();
            if(collisionAvoidCounter < Time.time)
            {
                enemyEnemyCollsion = false;
            }
        }
    }

    //move perpendicularly to get away from the collision
    public void navigateAround()
    {
        if (navigate_around_enemies)
        {  
            Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
            if (ramNavPos)
            {
                velocity = velocity + (new Vector2(-velocity.y, velocity.x).normalized);
            }
            else
            {
                velocity = velocity + (new Vector2(-velocity.y, velocity.x).normalized * -1);
            }
            GetComponent<Rigidbody2D>().velocity = velocity;
        }
    }


    public void Die()
    {
        SoundMixer.sound_manager.Play8bitExplosion();
        EffectsManager.effects.ViolentExplosion(this.transform.position);
        //TetherLightning.tether_lightning.BurstLightning((Vector2)this.transform.position + new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)), (Vector2) this.transform.position, 5);
        InGameUIManager.ui_manager.ChangeScore(pointValue, this.transform.position);
        EffectsManager.effects.GridExplosion(this.transform.position, 2f, 8f, Color.red);

        GameObject[] corpses = EffectsManager.effects.CutSprite(this.gameObject);

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
        return active;
        //if (GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
        // {
        //    return true;
        //}
        //else
        //{
        //     return false;
        // }
    }

    //check if enemy has entered the screen and must activate
    public void CheckActive()
    {
        //if (GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
       // {
       //     Activate();
        //}
    }

    //put any behaviour that must be done when the enemy enters the screen here
    public virtual void Activate()
    {
        active = true;

        // Turn on trails
        ManualTrail[] tr = this.GetComponentsInChildren<ManualTrail>(true);
        if (tr != null)
        {
            foreach (ManualTrail trail in tr)
            {
                trail.gameObject.SetActive(true);
            }
        }
    }

    //after activating the enemy should die if it leaves the screen
    public void CheckDeath()
    {
        /*
        if (!GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
        {
            DieOffScreen();
        }
        */

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

