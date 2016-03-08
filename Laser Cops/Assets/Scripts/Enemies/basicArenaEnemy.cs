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
	void Update () {
	
	}
}
