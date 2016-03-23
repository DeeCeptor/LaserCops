using UnityEngine;
using System.Collections;

public class BossHealthScript : MonoBehaviour {
    public float health = 50f;
	//number of stages the boss will have
	public int stages = 3;
	//number of health points for each stage transition to trigger on. highest health should be highest in the array
	public int[] stageHealthpoints;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    if(health <= 0)
        {
            Die();
        }
	}

	public void takeHit(float damage)
	{
		health -= damage;
	}

    public void Die()
    {
        Destroy(gameObject);
    }
}
