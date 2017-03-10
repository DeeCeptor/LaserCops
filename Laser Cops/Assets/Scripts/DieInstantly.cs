using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieInstantly : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<basicScrollingEnemyScript>().Die();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
