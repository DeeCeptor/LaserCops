using UnityEngine;
using System.Collections;

//this is a template for scrolling enemies and simply falls but has methods for doing more complex things
public class basicScrollingEnemyScript : MonoBehaviour
{
    public float speed = 0.25f;
    public bool active = false;
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
        }
        else
        {
            CheckDeath();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tether"))
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void initiate()
    {
        GetComponent<Rigidbody2D>().gravityScale = speed;
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
            Die();
        }
    }
}

