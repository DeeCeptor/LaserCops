using UnityEngine;
using System.Collections;

//will spawn the specified object on it's transform at a given time use a different script if you want the object to spawn new things repeatedly
public class SpawnAtTime : MonoBehaviour {
    //drag this in in unity's editor
    public GameObject objectToSpawn;
    public float secondsTillSpawn = 2f;
    private float spawnCounter = 0f;
    // Use this for initialization
    void Start () {
        spawnCounter = secondsTillSpawn + Time.time;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (spawnCounter < Time.time)
        {
            GameObject objectSpawned = (GameObject)Instantiate(objectToSpawn,transform.position,transform.rotation);
            objectSpawned.transform.SetParent(transform.parent);
            spawnCounter = float.PositiveInfinity;
        }
    }
}
