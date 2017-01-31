using UnityEngine;
using System.Collections;

public class AsteroidMaterializerOnCall : MonoBehaviour {

    public GameObject asteroid;
    GameObject newSpawn;

    public bool only_spawn_on_hard = false;

    void Start () {
	
	}


    void Update () {
	
	}

    public void MaterializeAsteroid()
    {
        if ((only_spawn_on_hard && GameState.game_state.current_difficulty == GameState.Difficulty.Hard) || !only_spawn_on_hard)
            newSpawn = (GameObject)Instantiate(asteroid,transform.position,transform.rotation);
    }
}
