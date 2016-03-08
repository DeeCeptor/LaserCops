using UnityEngine;
using System.Collections;

public class PlayerController : PlayerInput
{
    Rigidbody2D physics;

    void Awake ()
    {
        physics = this.GetComponent<Rigidbody2D>();
    }
	void Start ()
    {

    }

    void Update ()
    {
        UpdateInputs();

        physics.velocity = this.direction;
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
