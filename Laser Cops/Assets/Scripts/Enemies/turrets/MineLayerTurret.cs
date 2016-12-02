using UnityEngine;
using System.Collections;
//this script is for a turret that will shoot a mine at a player if they are in a horizontal line with the player. should be used for turrets on guys travelling up or down mostly
public class MineLayerTurret : ForwardShotScript {
    GameObject[] players ;

    //laser is to give players notice of the imminent shot the laser does not do damage and is spawned before the bullet
    public GameObject aimingLaser;
    public float laserLength = 60f;
    //length of time the laser will be present
    public float laserDuration = 0.1f;
    public float laserCounter = 0f;

    //the max amount of distance between the y position of the players and the y position of the turret for it to be allowed to fire
    public float allowedDistance = 1f;
    public bool playerInLine = false;
    public bool shooting = false;

    void Start ()
    {
        players = GameState.game_state.PlayerObjects;
    }


    void FixedUpdate ()
    {
        if (active)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].transform.position.y > transform.position.y - allowedDistance && players[i].transform.position.y < transform.position.y + allowedDistance)
                {
                    playerInLine = true;
                }
            }
            if (shotCounter < Time.time&&playerInLine && shooting == false)
            {
                shotCounter = Time.time + shotDelay;
                LayMine();
            }
            playerInLine = false;
            if(shooting == true)
            {
                shoot();
                shooting = false;
            }
        }
        else
        {
            checkActive();
        }
    }

    public void LayMine()
    {
        GameObject aimLaser = (GameObject)Instantiate(aimingLaser, transform.position, transform.rotation);
        aimLaser.transform.position = aimLaser.transform.position - (transform.up * laserLength);
        aimLaser.transform.SetParent(transform);
        laserCounter = Time.time + laserDuration;
        shooting = true;
    }
}
