using UnityEngine;
using System.Collections;

public class CivillianScript : MonoBehaviour {
	public int pointsForSave = 100;
	//they are subtracted from the score and should be positive
	public int pointPenaltyForKill = 200;
	public int pointPenaltyForAbandon = 0;
    public float healthToGainBack = 15f;
	//layer for the save ad destroy tether
	public int saveLayer = 13;
	public int destroyLayer = 12;

	public float speed = 2f;
	public bool active = false;

	private float inactiveSpeed = 1f;

	//direction the enemy will travel towards
	public direction travelDirection = direction.left;

    public bool switch_tether_text = true;  // If true, use the switch tether help text
    MeshRenderer switch_tether_mesh;

	void Start ()
    {
        if (switch_tether_text)
            switch_tether_mesh = this.GetComponentInChildren<MeshRenderer>();
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
            if (switch_tether_text)
                switch_tether_mesh.enabled = (Tether.tether.cur_tether_mode != Tether.TetherMode.Capture);

			CheckDeath();
			moveActive();
		}
	}

	public void moveInactive()
	{
		if(travelDirection == direction.left)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(-GameState.game_state.inactive_speed,0);
		}
		else if (travelDirection == direction.up)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, GameState.game_state.inactive_speed);
		}
		else if (travelDirection == direction.right)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(GameState.game_state.inactive_speed, 0);
		}
		else if (travelDirection == direction.down)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, -GameState.game_state.inactive_speed);
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
		if (GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
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
		if (!GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
		{
			Destroy(gameObject);
		}
	}

	public void Die()
	{
		Destroy(gameObject);
        InGameUIManager.ui_manager.ChangeScore(-pointPenaltyForAbandon, this.transform.position);
	}
	
	// Update is called once per frame
	public void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.layer == saveLayer)
		{
            Saved();
		}

		if(collision.gameObject.layer == destroyLayer)
		{
            EffectsManager.effects.spawnMovingText(new Vector3(this.transform.position.x, this.transform.position.y + 3, this.transform.position.z), "Killed Civilian!");
            EffectsManager.effects.ViolentExplosion(this.transform.position);
            InGameUIManager.ui_manager.ChangeScore(-pointPenaltyForKill, this.transform.position);
            Destroy(gameObject);
		}
	}


    public void Saved()
    {
        InGameUIManager.ui_manager.ChangeScore(pointsForSave, this.transform.position);
        EffectsManager.effects.spawnMovingText(new Vector3(this.transform.position.x, this.transform.position.y + 3, this.transform.position.z), "Saved!");
        EffectsManager.effects.PlayersHealed();
        SoundMixer.sound_manager.PlayCollectSound();

        GameState.game_state.Heal_All_Players(healthToGainBack);
            
        Destroy(gameObject);
    }
}
