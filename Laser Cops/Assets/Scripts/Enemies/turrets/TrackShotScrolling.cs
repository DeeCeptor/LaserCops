using UnityEngine;
using System.Collections;

//a turret that shoots at one of the players
public class TrackShotScrolling : MonoBehaviour{
    //will randomly shoot at one of the two players
    public Transform playerToTrack;
    public GameObject[] players;
    public float shotDelay = 0.5f;
    public float shotCounter = 0f;
    public GameObject bullet;
    public bool active = false;
    public _Colour bulletColour = _Colour.Red;

    //this boolean indicates whether the shots are useless against a certain car this is important since it needs to shoot at the car it's bullets are effective against
    public bool CarColour = false;

    // Use this for initialization
    void Start () {
        
        players = GameState.game_state.PlayerObjects;
        int randInt = Random.Range(0, players.Length);
        playerToTrack = players[randInt].transform;
        while(bulletColour == playerToTrack.GetComponent<PlayerController>().player_colour)
        {
            randInt = Random.Range(0, players.Length);
            playerToTrack = players[randInt].transform;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (active)
        {
            faceTarget(playerToTrack.position);
            if (shotCounter < Time.time)
            {
                shotCounter = Time.time + shotDelay;

                if (playerToTrack != null)
                {
                    shoot();
                    int randInt = Random.Range(0, players.Length);
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
        if(GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
        {
            Activate();
        }
    }

    public void faceTarget(Vector3 Target)
    {
        Vector3 vectorToTarget = Target - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle -90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime);
    }

    //activate the turret
    public void Activate()
    {
        active = true;
    }
    
    public void shoot()
    {
            GameObject bulletSpawned = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
            BulletScript bulletStats = bulletSpawned.GetComponent<BulletScript>();
            bulletStats.target = playerToTrack.position;
            bulletStats.bullet_colour = bulletColour;
            if(bulletColour == _Colour.Red)
            {
                bulletSpawned.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if (bulletColour == _Colour.Pink)
            {
                bulletSpawned.GetComponent<SpriteRenderer>().color = Color.magenta;
            }
            if (bulletColour == _Colour.Blue)
            {
                bulletSpawned.GetComponent<SpriteRenderer>().color = Color.cyan;
            }
            if (bulletColour == _Colour.Yellow)
            {
                bulletSpawned.GetComponent<SpriteRenderer>().color = Color.yellow;
            }

            SoundMixer.sound_manager.PlayLazerShot();
    }
}
