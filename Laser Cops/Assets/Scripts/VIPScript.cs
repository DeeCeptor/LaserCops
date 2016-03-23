using UnityEngine;
using System.Collections;

public class VIPScript : MonoBehaviour {

	public float health = 30f;
	public int destructiveTetherLayer = 12;
	//damage taken from the destructive tether
	public float friendlyFireDamage = 0.2f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		if(collision.gameObject.layer == destructiveTetherLayer)
		{
			TakeHit(friendlyFireDamage);
		}
	}

	public void TakeHit(float damage)
	{
		health -= damage;
		if(health <= 0)
		{
			Die();
		}
	}

	public void Die()
	{
		Destroy(gameObject);
		//todo lose game when we have a gameover screen
	}
}
