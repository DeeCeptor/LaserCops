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
	public int pointValue = 20;
	public float health = 1f;

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

    public void Die()
    {
        SoundMixer.sound_manager.Play8bitExplosion();
        EffectsManager.effects.ViolentExplosion(this.transform.position);
        TetherLightning.tether_lightning.BurstLightning((Vector2)this.transform.position + new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)), (Vector2) this.transform.position);
        UIManager.ui_manager.ChangeScore(pointValue);
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

