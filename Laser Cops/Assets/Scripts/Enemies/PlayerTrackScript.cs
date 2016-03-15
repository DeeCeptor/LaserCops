using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//this script is just to track the players so that I don't have to call GameObject. findObject with tag all the time as it's inefficient
public class PlayerTrackScript : MonoBehaviour {

    public static PlayerTrackScript playerInfo;
    public  GameObject[] players;
	// Use this for initialization
	void Start () {
        playerInfo = this;
        players = GameObject.FindGameObjectsWithTag("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	for(int i = 0; i < players.Length;i++)
        {
            if (players[i] == null)
            {
                WhenPlayersDie();
            }
        }
	}

    public void WhenPlayersDie()
    {
        players = new GameObject[0];
        // do this when we have a respawn system
    }
}
