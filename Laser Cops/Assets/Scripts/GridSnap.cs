using UnityEngine;
using System.Collections;
using UnityEditor;

// https://www.youtube.com/watch?v=ukYbRmmlaTM
[ExecuteInEditMode]
public class GridSnap : MonoBehaviour
{
    public bool remove_on_difficulty_higher_than = false;
    public GameState.Difficulty difficulty = GameState.Difficulty.Easy;
    Vector3 offset = new Vector3(-1, 1, 0);

    public float cell_size = 1f;
    private float x, y, z;

    // Camera distance
    Vector3 minScreenBounds;
    Vector3 maxScreenBounds;
    string distance = "";
    Vector3 prev_position;

    void Start()
    {
        x = 0f;
        y = 0f;
        z = 0f;

        if (EditorApplication.isPlaying)
        {
            if (!remove_on_difficulty_higher_than)
            {
                switch (difficulty)
                {
                    case GameState.Difficulty.Easy:
                        break;
                    case GameState.Difficulty.Normal:
                        if (GameState.game_state.current_difficulty == GameState.Difficulty.Easy)
                            DestroyImmediate(this.gameObject);
                        break;
                    case GameState.Difficulty.Hard:
                        if (GameState.game_state.current_difficulty == GameState.Difficulty.Easy
                            || GameState.game_state.current_difficulty == GameState.Difficulty.Normal)
                            DestroyImmediate(this.gameObject);
                        break;
                }
            }
            else
            {
                switch (difficulty)
                {
                    case GameState.Difficulty.Easy:
                        if (GameState.game_state.current_difficulty != GameState.Difficulty.Easy)
                            DestroyImmediate(this.gameObject);
                        break;
                    case GameState.Difficulty.Normal:
                        if (GameState.game_state.current_difficulty == GameState.Difficulty.Hard)
                            DestroyImmediate(this.gameObject);
                        break;
                    case GameState.Difficulty.Hard:
                        break;
                }
            }
         }
    }

    #if (UNITY_EDITOR)
    void Update()
    {
        if (!Application.isPlaying)
        {
            x = Mathf.Round(transform.position.x / cell_size) * cell_size;
            y = Mathf.Round(transform.position.y / cell_size) * cell_size;
            z = transform.position.z;
            transform.position = new Vector3(x, y, z);

            if (minScreenBounds == Vector3.zero)
            {
                maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
                minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            }
        }
    }

    void OnDrawGizmos()
    {
        if (distance == "" || prev_position != this.transform.position)
        {
            prev_position = this.transform.position;

            // Calculate distance to nearest camera view
            float dist = 0f;
            if (this.transform.position.x > maxScreenBounds.x)
            {
                // Is to the right of the camera
                dist = Mathf.Abs(this.transform.position.x) - maxScreenBounds.x;
            }
            else if (this.transform.position.x < minScreenBounds.x)
            {
                // Left of camera
                dist = Mathf.Abs(this.transform.position.x) + minScreenBounds.x;
            }
            else if (this.transform.position.y > maxScreenBounds.y)
            {
                // Above camera
                dist = Mathf.Abs(this.transform.position.y) - maxScreenBounds.y;
            }
            else if (this.transform.position.y < minScreenBounds.y)
            {
                // Below camera
                dist = Mathf.Abs(this.transform.position.y) + minScreenBounds.y;
            }
            distance = "" + Mathf.Round(dist);
        }

        if (distance != "" + 0)
            UnityEditor.Handles.Label(this.transform.position, distance);

        UnityEditor.Handles.Label(this.transform.position + offset, difficulty + "");
    }
    #endif
}
