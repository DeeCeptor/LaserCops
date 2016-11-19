using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    basicScrollingEnemyScript script;
    public GameObject object_to_spawn;
    public GameObject spawn_position;
    public int number_of_objects_to_spawn;
    public float time_between_spawning = 2f;
    float cur_time_between_spawning = 0;

    void Awake ()
    {
        script = this.GetComponent<basicScrollingEnemyScript>();
	}
	

	void Update ()
    {
        if (script.active)
        {
            cur_time_between_spawning -= Time.deltaTime;

            if (number_of_objects_to_spawn > 0 && cur_time_between_spawning <= 0)
            {
                number_of_objects_to_spawn--;
                GameObject obj = (GameObject) Instantiate(object_to_spawn);
                obj.transform.position = spawn_position.transform.position;
                obj.AddComponent<EnableCollider>();
                cur_time_between_spawning = time_between_spawning;
            }
        }
	}
}
