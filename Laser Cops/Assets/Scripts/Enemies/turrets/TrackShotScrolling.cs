using UnityEngine;
using System.Collections;

//a turret that shoots at one of the players
public class TrackShotScrolling : MonoBehaviour{
    //will randomly shoot at one of the two players
    private Transform playerToTrack;
    private GameObject[] players;
    public float shotDelay = 0.5f;
    public float shotCounter;
    public GameObject bullet;
    public bool active = false;

    //true if you want it to disable itself when the player is close
    public bool playerCloseDisable = true;
    //how close the player must be to disable the shot
    public float disableDistance = 2f;

    // Use this for initialization
    void Start () {
        players = GameState.game_state.PlayerObjects;
        int randInt = Random.Range(0, players.Length);
        playerToTrack = players[randInt].transform;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (active)
        {
            if (shotCounter < Time.time)
            {
                shotCounter = Time.time + shotDelay;

                if (playerToTrack != null)
                {
                    shoot();
                }

                else
                {
                    players = GameState.game_state.PlayerObjects;
                    if (players.Length > 0)
                    {
                        int randInt = Random.Range(0, players.Length);
                        playerToTrack = players[randInt].transform;
                        shoot();
                    }
                }
            }
        }
        else
        {
            checkActive();
        }
    }

    //see if the turret needs to activate
    public void checkActive()
    {
        if(GetComponent<SpriteRenderer>().isVisible)
        {
            Activate();
        }
    }

    //activate the turret
    public void Activate()
    {
        active = true;
        shotCounter = Time.time + shotDelay;
    }
    
    public void shoot()
    {
        if (!playerCloseDisable)
        {
            GameObject bulletSpawned = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
            BulletScript bulletStats = bulletSpawned.GetComponent<BulletScript>();
            bulletStats.target = playerToTrack.position;
        }

        else
        {
            bool fire = true;
			for (int i = 0; i < GameState.game_state.Players.Count; i++)
            {
				if ((GameState.game_state.Players[i].transform.position - transform.position).magnitude < disableDistance)
                {
                    fire = false;
                }
            }
            if (fire)
            {
                GameObject bulletSpawned = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
                BulletScript bulletStats = bulletSpawned.GetComponent<BulletScript>();
                bulletStats.target = playerToTrack.position;
            }
        }
    }
}
