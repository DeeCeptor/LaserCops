using UnityEngine;
using System.Collections;

public class basicScrollingEnemyScript : MonoBehaviour {
    public float speed = 0.25f;
	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody2D>().gravityScale = speed;
	}
	
	// Update is called once per frame
	void Update () {
	
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
