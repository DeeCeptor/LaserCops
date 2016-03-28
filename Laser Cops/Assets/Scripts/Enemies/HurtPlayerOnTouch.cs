using UnityEngine;
using System.Collections;

public class HurtPlayerOnTouch : MonoBehaviour
{
    public float collisionDamage = 1.0f;


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
        if (collision.gameObject.tag == "Player")
        {
            // Hurt the player
            collision.gameObject.GetComponent<PlayerController>().TakeHit(collisionDamage);
        }

        else if (collision.gameObject.tag == "VIP")
        {
            collision.gameObject.GetComponent<VIPScript>().TakeHit(collisionDamage);
        }
    }   
}
