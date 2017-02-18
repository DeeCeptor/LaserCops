using UnityEngine;
using System.Collections;

public class basicArenaEnemy : MonoBehaviour
{
    public float speed = 3f;
    public Transform playerToTrack;
    public GameObject[] players;
    public bool die_in_one_hit = false;
    public bool takes_grinding_damage = false;
    public int pointValue = 20;
	public float health = 50f;
    public bool active = false;

    //this is used for the enemies speed when OFFSCREEN do not change unless you know what you're doing in which case I'm a comment not a cop
    public float inactiveSpeed = 1f;

    //direction the enemy will travel towards
    public direction travelDirection = direction.left;

    //direction to move in once chasing player used internally but may be used by scripts that inherit this script
    public Vector2 dir;

    public float collisionDamage = 1f;
    protected float tether_lightning_cooldown = 0;
    protected float tether_lightning_delay = 0.3f;

    void Start ()
    {
        players = GameState.game_state.PlayerObjects;
        int randInt = Random.Range(0,players.Length);
        playerToTrack = players[randInt].transform;
	}


    void Update()
    {
        tether_lightning_cooldown -= Time.deltaTime;
    }

    void FixedUpdate ()
    {
        if (active == false)
        {
            CheckActive();
            moveInactive();
        }
        else
        {
            
            Follow();
            CheckDeath();
        }
    }


    public void Follow()
    {
        if (playerToTrack == null)
        {
            SelectTarget();
        }

        dir = playerToTrack.position - transform.position;
        GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
    }


    public void SelectTarget()
    {
        players = GameState.game_state.PlayerObjects;
        int randInt = Random.Range(0, players.Length);
        playerToTrack = players[randInt].transform;
    }

    public void CheckDeath()
	{
		if(health<=0)
		{
			Die();
		}
	}

	public void TakeHit(float damage)
	{
		health -= damage;
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

    public void moveInactive()
    {
        
        if (travelDirection == direction.left)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-inactiveSpeed, 0);
        }
        else if (travelDirection == direction.up)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, inactiveSpeed);
        }
        else if (travelDirection == direction.right)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(inactiveSpeed, 0);
        }
        else if (travelDirection == direction.down)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -inactiveSpeed);
        }
    }

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

    //to be used whenthe enemy dies offscreen
    public void DieOffScreen()
	{
		Destroy(gameObject);
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
}
