using UnityEngine;
using System.Collections;

public class BossHealthScript : MonoBehaviour {
    public float health = 50f;
    //if this field is empty the boss will die when out of health. otherwise the boss will change form when it runs out of health
    public GameObject nextStage;
	//number of stages the boss will have
	public int stages = 3;
	//number of health points for each stage transition to trigger on. highest health should be highest in the array
	public int[] stageHealthpoints;

	// Use this for initialization
	void Start ()
    {
        UIManager.ui_manager.ActivateBottomHealthBar("Welcome to the GUNSHIP", Color.red, 50);
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
        UIManager.ui_manager.UpdateBottomHealthBar(health);
	}

    public void Die()
    {
        Destroy(gameObject);
        if(nextStage!=null)
        {
            Instantiate(nextStage,transform.position,transform.rotation);
        }
    }
}
