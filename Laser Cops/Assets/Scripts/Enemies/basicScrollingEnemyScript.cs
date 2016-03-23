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
	public float collisionDamage = 0.3f;
	public int pointValue = 20;
	public float health = 1f;
    
    //this is used for the enemies speed when OFFSCREEN do not change unless you know what you're doing in which case I'm a comment not a cop
    private float inactiveSpeed = 1f;

    //direction the enemy will travel towards
    public direction travelDirection = direction.left;

    // Use this for initialization
    void Start()
    {
        initiate();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
        if (collision.gameObject.CompareTag("Tether"))
        {
            Die();
        }

		else if (collision.gameObject.tag == "Player")
		{
			collision.gameObject.GetComponent<PlayerController>().TakeHit( collisionDamage);
		}

		else if (collision.gameObject.tag == "VIP")
		{
			collision.gameObject.GetComponent<VIPScript>().TakeHit( collisionDamage);
		}
    }

    public void moveInactive()
    {
        if(travelDirection == direction.left)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-inactiveSpeed,0);
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

    public void moveActive()
    {
        if (travelDirection == direction.left)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, 0);
        }
        else if (travelDirection == direction.up)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
        }
        else if (travelDirection == direction.right)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
        }
        else if (travelDirection == direction.down)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -speed);
        }
    }

    public void Die()
    {
        EffectsManager.effects.ViolentExplosion(this.transform.position);
        Destroy(gameObject);
		UIManager.ui_manager.ChangeScore(-pointValue);
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
        if (GetComponent<SpriteRenderer>().isVisible)
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
        if (GetComponent<SpriteRenderer>().isVisible)
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
        if (!GetComponent<SpriteRenderer>().isVisible)
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

