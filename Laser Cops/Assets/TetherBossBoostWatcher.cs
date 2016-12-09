using UnityEngine;
using System.Collections;

//This script watches for when the player boosts and then reassigns the player layer. Only to gbe used for the tether boss
public class TetherBossBoostWatcher : MonoBehaviour {
    public PlayerController playerControls;
    public bool boosting = false;
	// Use this for initialization
	void Start () {
        playerControls = gameObject.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(playerControls.currently_boosting && !boosting)
        {
            boosting = true;
            gameObject.layer = 21;
        }

        else if (!playerControls.currently_boosting && boosting)
        {
            boosting = false;
            gameObject.layer = 10;
        }
    }
}
