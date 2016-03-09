using UnityEngine;
using System.Collections;

public class basicScrollingEnemyScript : MonoBehaviour
{
    public float speed = 0.25f;


    void Start ()
    {
        GetComponent<Rigidbody2D>().gravityScale = speed;
	}


    void Update ()
    {
	
	}


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Tether"))
        {
            Die();
        }
    }


    public void Die()
    {
        Destroy(gameObject);
    }
}
