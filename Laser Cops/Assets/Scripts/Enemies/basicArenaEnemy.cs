using UnityEngine;
using System.Collections;

public class basicArenaEnemy : MonoBehaviour {
    public float speed = 0.5f;
    private Transform playerToTrack;
    private GameObject[] players;
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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tether"))
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
