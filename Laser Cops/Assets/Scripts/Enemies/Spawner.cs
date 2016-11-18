using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    basicScrollingEnemyScript script;
    public GameObject object_to_spawn;
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
                Instantiate(object_to_spawn, this.transform.position, Quaternion.identity);
                cur_time_between_spawning = time_between_spawning;
            }
        }
	}
}
