﻿using UnityEngine;
using System.Collections;

public class CivillianScript : MonoBehaviour {
	public int pointsForSave = 100;
	//they are subtracted from the score and should be positive
	public int pointPenaltyForKill = 200;
	public int pointPenaltyForAbandon = 0;
	//layer for the save ad destroy tether
	public int saveLayer = 13;
	public int destroyLayer = 12;

	public float speed = 2f;
	public bool active = false;

	private float inactiveSpeed = 1f;

	//direction the enemy will travel towards
	public direction travelDirection = direction.left;

	// Use this for initialization
	void Start () {
	
	}

	void FixedUpdate()
	{
		if (!active)
		{
			CheckActive();
			moveInactive();
		}
		else
		{
			CheckDeath();
			moveActive();
		}
	}

	public void moveInactive()
	{
		if(travelDirection == direction.left)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(-inactiveSpeed,0);
		}
		else if (travelDirection == direction.up)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, inactiveSpeed);
		}
		else if (travelDirection == direction.right)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(inactiveSpeed, 0);
		}
		else if (travelDirection == direction.down)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, -inactiveSpeed);
		}
	}

	public void moveActive()
	{
		if (travelDirection == direction.left)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, 0);
		}
		else if (travelDirection == direction.up)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
		}
		else if (travelDirection == direction.right)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
		}
		else if (travelDirection == direction.down)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, -speed);
		}
	}

	public void CheckActive()
	{
		if (GetComponent<SpriteRenderer>().isVisible)
		{
			Activate();
		}
	}

	//put any behaviour that must be done when the enemy enters the screen here
	public void Activate()
	{
		active = true;
	}

	//after activating the enemy should die if it leaves the screen
	public void CheckDeath()
	{
		if (!GetComponent<SpriteRenderer>().isVisible)
		{
			Die();
		}
	}

	public void Die()
	{
		Destroy(gameObject);
		UIManager.ui_manager.ChangeScore(-pointPenaltyForAbandon);
	}
	
	// Update is called once per frame
	public void OnCollisionStay2D(Collision2D collision)
	{
		if(collision.gameObject.layer == 13)
		{
			Destroy(gameObject);
			UIManager.ui_manager.ChangeScore(pointsForSave);
		}

		if(collision.gameObject.layer == 12)
		{
			Destroy(gameObject);
			UIManager.ui_manager.ChangeScore(-pointPenaltyForKill);
		}
	}
}