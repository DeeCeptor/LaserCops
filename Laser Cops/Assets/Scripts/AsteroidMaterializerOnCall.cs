using UnityEngine;
using System.Collections;

public class AsteroidMaterializerOnCall : MonoBehaviour {

    public GameObject asteroid;
    GameObject newSpawn;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void MaterializeAsteroid()
    {
        newSpawn = (GameObject)Instantiate(asteroid,transform.position,transform.rotation);
    }
}
