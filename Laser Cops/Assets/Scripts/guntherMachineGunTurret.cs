using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class guntherMachineGunTurret : TrackShotScrolling {
    public float changeColourCounter = 0f;
    public float changeColourTimer = 5f;
    public GameObject blueBullet;
    public GameObject pinkBullet;

	// Use this for initialization
	void Start () {
        players = GameState.game_state.PlayerObjects;
        int randInt = Random.Range(0, players.Length);
        playerToTrack = players[randInt].transform;
        while (bulletColour == playerToTrack.GetComponent<PlayerController>().player_colour)
        {
            randInt = Random.Range(0, players.Length);
            playerToTrack = players[randInt].transform;
        }

        changeColourCounter = Time.time + changeColourTimer;
    }
	
	// Update is called once per frame
	void Update () {
		if(changeColourCounter < Time.time)
        {
            if(bulletColour == _Colour.Blue)
            {
                bulletColour = _Colour.Pink;
                bullet = pinkBullet;
            }

            else if (bulletColour == _Colour.Pink)
            {
                bulletColour = _Colour.Blue;
                bullet = blueBullet;
            }

            int randInt = Random.Range(0, players.Length);
            playerToTrack = players[randInt].transform;
            while (bulletColour == playerToTrack.GetComponent<PlayerController>().player_colour)
            {
                randInt = Random.Range(0, players.Length);
                playerToTrack = players[randInt].transform;
            }
            changeColourCounter = Time.time + changeColourTimer;
        }
	}

    
}
