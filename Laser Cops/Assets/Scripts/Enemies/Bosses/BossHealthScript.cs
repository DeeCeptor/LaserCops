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
    public string bossName = "Welcome to the GUNSHIP";

    // Use this for initialization
    void Start ()
    {
        
        if(!InGameUIManager.ui_manager.bottom_bar.activeInHierarchy)
        {
            InGameUIManager.ui_manager.ActivateBottomHealthBar(bossName, Color.red, overallHealth);
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
        InGameUIManager.ui_manager.UpdateBottomHealthBar(overallHealth);
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "BounceBomb")
        {
            takeHit(collision.gameObject.GetComponent<bounceBomb>().damageToBoss);
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        if(nextStage!=null)
        {
            Instantiate(nextStage,transform.position,transform.rotation);
        }
        else
        {
            GameState.game_state.Victory();
        }
    }
}
