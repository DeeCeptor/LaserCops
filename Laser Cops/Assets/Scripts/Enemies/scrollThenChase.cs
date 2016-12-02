using UnityEngine;
using System.Collections;

public class scrollThenChase : ScrollThenStop
{
    public float speedAfterStop = 2f;
    public Transform playerToTrack;
    public GameObject[] players;
    //direction to move in once chasing player used internally but may be used by scripts that inherit this script
    public Vector2 dir;


    void FixedUpdate ()
    {
        tether_lightning_cooldown -= Time.deltaTime;

        if (!active)
        {
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
                    stopped = true;
                }
            }
            else
            {
                Chase();
                if (goAgain && stopped)
                {
                        unfreeze();
                        speed = speedAfterStop;
                    stopped = true;
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
        desired_velocity = dir.normalized * speed;
        GetComponent<Rigidbody2D>().velocity = desired_velocity;
    }

    public void SelectTarget()
    {
        players = GameState.game_state.PlayerObjects;
        int randInt = Random.Range(0, players.Length);
        playerToTrack = players[randInt].transform;
    }
}
