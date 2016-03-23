using UnityEngine;
using System.Collections;

public class basicArenaEnemy : MonoBehaviour {
    public float speed = 0.5f;
    private Transform playerToTrack;
    private GameObject[] players;
	public int pointValue = 20;
	public float health = 50f;

	public float collisionDamage = 1f;
	// Use this for initialization
	void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        int randInt = Random.Range(0,players.Length);
        playerToTrack = players[randInt].transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (playerToTrack != null)
        {
            Vector2 dir = playerToTrack.position - transform.position;
            GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
        }

        else
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > 0)
            {
                int randInt = Random.Range(0, players.Length);
                playerToTrack = players[randInt].transform;
            }
        }
		CheckDeath();
    }

	public void CheckDeath()
	{
		if(health<=0)
		{
			Die();
		}
	}

	public void TakeHit(float damage)
	{
		health -= damage;
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tether"))
        {
            Die();
        }

		else if (collision.gameObject.tag == "Player")
		{
			collision.gameObject.GetComponent<PlayerController>().TakeHit( collisionDamage);
		}

		else if (collision.gameObject.tag == "VIP")
		{
			collision.gameObject.GetComponent<VIPScript>().TakeHit( collisionDamage);
		}
    }

	public void Die()
	{
		EffectsManager.effects.ViolentExplosion(this.transform.position);
		Destroy(gameObject);
		UIManager.ui_manager.ChangeScore(-pointValue);
	}

	//to be used whenthe enemy dies offscreen
	public void DieOffScreen()
	{
		Destroy(gameObject);
	}
}
