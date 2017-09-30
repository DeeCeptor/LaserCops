using UnityEngine;
using System.Collections;

public class CivillianScript : MonoBehaviour
{
	public int pointsForSave = 100;
	// They are subtracted from the score and should be positive
	int pointPenaltyForKill = -200;
	public int pointPenaltyForAbandon = 0;
    public float healthToGainBack = 15f;
	// Layer for the save and destroy tether
	public int saveLayer = 13;
	public int destroyLayer = 12;

	public float speed = 2f;
	public bool active = false;

	private float inactiveSpeed = 1f;

    public bool shrinking = false;

	//direction the enemy will travel towards
	public direction travelDirection = direction.left;

    public bool switch_tether_text = true;  // If true, use the switch tether help text
    MeshRenderer switch_tether_mesh;

	void Start ()
    {
        if (GameState.game_state.game_mode == GameState.GameMode.Competitive)
            Destroy(this.gameObject);

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
            if (!shrinking)
            {
                if (switch_tether_text)
                    switch_tether_mesh.enabled = (Tether.tether.cur_tether_mode != Tether.TetherMode.Capture);

                CheckDeath();
                moveActive();
            }
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
        InGameUIManager.ui_manager.ChangeScore(pointPenaltyForKill, this.transform.position);
        SoundMixer.sound_manager.Play8bitExplosion();
        EffectsManager.effects.ViolentExplosion(this.transform.position);
        EffectsManager.effects.GridExplosion(this.transform.position, 2f, 8f, Color.red);

        GameObject[] corpses = EffectsManager.effects.CutSprite(this.gameObject);

        Destroy(gameObject);
    }


    public void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.layer == saveLayer)
		{
            Saved();
		}

		if(collision.gameObject.layer == destroyLayer)
		{
            Die();
		}
	}


    public void Saved()
    {
        if (shrinking)
            return;

        switch_tether_text = false;
        shrinking = true;
        InGameUIManager.ui_manager.ChangeScore(pointsForSave, this.transform.position);
        EffectsManager.effects.spawnMovingText(new Vector3(this.transform.position.x, this.transform.position.y + 3, this.transform.position.z), "Saved!");
        EffectsManager.effects.PlayersHealed();
        SoundMixer.sound_manager.PlayCollectSound();
        TetherLightning.tether_lightning.BurstLightning(this.transform.position, this.transform.position + new Vector3(0, 2), 20, Color.green);

        // Heal all players
        GameState.game_state.Heal_All_Players(healthToGainBack);

        StartCoroutine(Shrinking_Animation());
    }

    float fly_away_speed = 15f;
    IEnumerator Shrinking_Animation()
    {
        float time_remaining = 8f;
        // Disable the collider, so it can warp away
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.GetComponent<Rigidbody2D>().freezeRotation = true;
        foreach (TrailRenderer rend in this.GetComponentsInChildren<TrailRenderer>(true))
            rend.gameObject.SetActive(true);

        while (time_remaining >= 0)
        {
            this.transform.position -= new Vector3(Time.deltaTime * fly_away_speed, 0, 0);
            time_remaining -= Time.deltaTime;
            yield return 0;
        }
        /*
        while (this.transform.localScale.x >= 0)
        {
            this.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, 0);
            yield return 0;
        }*/

        Destroy(gameObject);
        yield return null;
    }
}
