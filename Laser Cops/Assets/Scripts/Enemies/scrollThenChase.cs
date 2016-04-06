using UnityEngine;
using System.Collections;

public class scrollThenChase : ScrollThenStop
{
    public float speedAfterStop = 2f;
    private Transform playerToTrack;
    public GameObject[] players;
    //direction to move in once chasing player used internally but may be used by scripts that inherit this script
    public Vector2 dir;

    void Start ()
    {
	
	}


    void FixedUpdate ()
    {
        tether_lightning_cooldown -= Time.deltaTime;

        if (!active)
        {
            CheckActive();
            moveInactive();
        }
        else
        {
            CheckDeath();
            
            if (!stopped)
            {
                moveActive();
                if (stopCounter < Time.time)
                {
                    FreezePosition();
                }
            }
            else
            {
                Chase();
                if (goAgain)
                {
                    if (stopCounter < Time.time)
                    {
                        unfreeze();
                        speed = speedAfterStop;
                    }
                }
            }
        }
    }

    public void Chase()
    {
        if(playerToTrack == null)
        {
            SelectTarget();
        }

        dir = playerToTrack.position - transform.position;
        GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
    }

    public void SelectTarget()
    {
        players = GameState.game_state.PlayerObjects;
        int randInt = Random.Range(0, players.Length);
        playerToTrack = players[randInt].transform;
    }
}
