using UnityEngine;
using System.Collections;

//destroy object after a certain number of seconds
public class DieOverTime : MonoBehaviour {

    public float secondsTillDeath = 2f;
    private float deathCounter = 0f;
	// Use this for initialization
	void Start () {
        deathCounter = secondsTillDeath + Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    if(deathCounter < Time.time)
        {
            Destroy(gameObject);
        }
	}
}
