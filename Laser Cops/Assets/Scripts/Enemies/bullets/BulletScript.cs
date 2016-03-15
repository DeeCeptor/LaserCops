using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
    public Vector3 target;
    public float speed = 3f;
    public Vector2 dir;
	// Use this for initialization
	void Start () {
        dir = target - transform.position;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        
        GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
