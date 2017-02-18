using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackShotDelayBeforeFire : TrackShotScrolling {

	// Use this for initialization
	void Start () {
        shotCounter = Time.time + shotDelay - shotCounter;
        players = GameState.game_state.PlayerObjects;
        int randInt = Random.Range(0, players.Length);
        playerToTrack = players[randInt].transform;
    }
}
