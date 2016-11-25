using UnityEngine;
using System.Collections.Generic;

public class LevelSelectCamera : MonoBehaviour 
{
    public List<GameObject> camera_positions;
    Vector3 cur_pos;
    float normal_size;
    float zoomed_size = 3f;

    float speed = 5;


	void Start () 
	{
        cur_pos = this.transform.position;
        normal_size = Camera.main.orthographicSize;
    }
	

	void Update () 
	{
        // Zooming in, move towards level
	    if (LevelManager.level_manager.level_settings.activeSelf)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, zoomed_size, speed * Time.deltaTime);
            this.transform.position = Vector2.Lerp(this.transform.position, (Vector2)LevelManager.level_manager.selected_level_ui.transform.position + new Vector2(0, -0.5f), speed * Time.deltaTime);
        }
        // Zooming out
        else
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, normal_size, speed * Time.deltaTime);
            this.transform.position = Vector2.Lerp(this.transform.position, cur_pos, speed * Time.deltaTime);
        }


        // Find the closest camera position
        if (LevelManager.level_manager.selected_level_ui.activeSelf || true)
        {
            float closest_distance = 9999;
            GameObject closest_position = null;

            foreach(GameObject pos in camera_positions)
            {
                float dist = Vector2.Distance(pos.transform.position, PlayerCursor.cursor.transform.position);
                if (dist < closest_distance)
                {
                    closest_distance = dist;
                    closest_position = pos;
                }
            }

            // Set new position
            if (closest_position != null)
                cur_pos = closest_position.transform.position;
        }
    }
}
