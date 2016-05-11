using UnityEngine;
using System.Collections;

public class LevelSelectCamera : MonoBehaviour 
{
    Vector3 start_pos;
    float normal_size;
    float zoomed_size = 3f;

    float speed = 5;

	void Start () 
	{
        start_pos = this.transform.position;
        normal_size = Camera.main.orthographicSize;
    }
	

	void Update () 
	{
        // Zooming in, move towards level
	    if (LevelManager.level_manager.level_settings.activeSelf)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, zoomed_size, speed * Time.deltaTime);
            this.transform.position = Vector2.Lerp(this.transform.position, LevelManager.level_manager.selected_level_ui.transform.position, speed * Time.deltaTime);
        }
        // Zooming out
        else
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, normal_size, speed * Time.deltaTime);
            this.transform.position = Vector2.Lerp(this.transform.position, start_pos, speed * Time.deltaTime);
        }
    }
}
