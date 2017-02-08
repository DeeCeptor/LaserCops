using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LevelSelectCamera : MonoBehaviour 
{
    public List<GameObject> camera_positions;
    Vector3 cur_pos;
    float normal_size;
    float zoomed_size = 3f;

    float speed = 5;

    public List<Material> skyboxes;
    public Skybox cur_skybox;
    public SpriteRenderer fading_blocker;

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

            foreach (GameObject pos in camera_positions)
            {
                float dist = Vector2.Distance(pos.transform.position, PlayerCursor.cursor.transform.position);
                if (dist < closest_distance)
                {
                    closest_distance = dist;
                    closest_position = pos;
                }
            }

            // Change to new position
            if (closest_position.transform.position != cur_pos)
            {
                Debug.Log("Moving to new zone");
                cur_pos = closest_position.transform.position;

                // Start fading out skybox routine
                // Set new skybox
                StartCoroutine(fade_new_skybox(0.3f, skyboxes[camera_positions.IndexOf(closest_position)]));
            }
        }
    }


    public IEnumerator fade_new_skybox(float time, Material new_skybox)
    {
        while (fading_blocker.color.a < 1)
        {
            Color c = fading_blocker.color;
            c.a += Time.deltaTime * (1f / time);
            fading_blocker.color = c;
            yield return 0;
        }

        // Assign new skybox now that we've completely blocked out that camera
        cur_skybox.material = new_skybox;

        while (fading_blocker.color.a > 0)
        {
            Color c = fading_blocker.color;
            c.a -= Time.deltaTime * (1f / time);
            fading_blocker.color = c;
            yield return 0;
        }
    }
}
