using UnityEngine;
using System.Collections;

public class BossHealthScript : MonoBehaviour {
    public float health = 50f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    if(health < 0)
        {
            Die();
        }
	}

    public void Die()
    {
        Destroy(gameObject);
    }
}
