using UnityEngine;
using System.Collections;

public class BossHealthScript : MonoBehaviour {
    public float health = 20f;
    //if this field is empty the boss will die when out of health. otherwise the boss will change form when it runs out of health
    public GameObject nextStage;
	//number of stages the boss will have
	public int stages = 3;
	//number of healthpoints remaining for the boss overall(across multiple forms)
	public float overallHealth = 60;

	// Use this for initialization
	void Start ()
    {
        
        if(!UIManager.ui_manager.bottom_bar.activeInHierarchy)
        {
            UIManager.ui_manager.ActivateBottomHealthBar("Welcome to the GUNSHIP", Color.red, overallHealth);
        }
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
        overallHealth -= damage;
        UIManager.ui_manager.UpdateBottomHealthBar(overallHealth);
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
