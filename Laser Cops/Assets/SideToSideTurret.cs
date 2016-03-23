using UnityEngine;
using System.Collections;

//moves a turret side to side will start by incresing x or y value
public class SideToSideTurret : MonoBehaviour {

    public float speed = 0.25f;
    //how many units to move before changing directions
    public float limit = 2f;
    public float limitCounter = 0f;
    //whether it has horizontal or vertical back and forth movement
    public bool horizontal = true;
    //whether the object is currently increasing in x or y value
    bool up = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    
	}
}
