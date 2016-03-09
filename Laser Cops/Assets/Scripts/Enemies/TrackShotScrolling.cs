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
    // Use this for initialization
    void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
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
                    players = GameObject.FindGameObjectsWithTag("Player");
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
        GameObject bulletSpawned = (GameObject)Instantiate(bullet,transform.position,transform.rotation);
        BulletScript bulletStats = bulletSpawned.GetComponent<BulletScript>();
        bulletStats.target = playerToTrack.position;
    }
}
