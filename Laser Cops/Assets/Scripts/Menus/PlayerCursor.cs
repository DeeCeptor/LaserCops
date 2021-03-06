using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using InControl;

public class PlayerCursor : MonoBehaviour 
{
    public LevelNode starting_node;
    public LevelNode hovering_over_level;
    public LevelNode destination_node;
    public GameObject options;

    public static PlayerCursor cursor;

    float min_y_difference = 1f;
    float min_x_difference = 1f;
    float move_speed = 6f;


    void Awake()
    {
        cursor = this;
    }
	void Start () 
	{
        bool found_last_level = false;
        string last_level = PlayerPrefs.GetString("LastLevelPlayed");
        // Search for the last level played and start the cursor there
        if (!string.IsNullOrEmpty(last_level))
        {
            Debug.Log("Last level played: " + last_level);

            foreach (LevelNode ln in LevelManager.level_manager.level_nodes)
            {
                if (ln.level_to_load == last_level)
                {
                    Debug.Log("Found last level");
                    starting_node = ln;
                    hovering_over_level = ln;
                    this.transform.position = ln.transform.position;
                    destination_node = ln;
                    found_last_level = true;
                }
            }
        }

        if(!found_last_level)
        {
            Debug.Log("Could not find last level");
            hovering_over_level = starting_node;
            this.transform.position = starting_node.transform.position;
            destination_node = starting_node;
        }
    }
	

    public void SetNewDestination(LevelNode new_dest)
    {
        if (!new_dest && new_dest != this)
            return;

        LevelManager.level_manager.DeselectLevels();
        destination_node = new_dest;
    }


	void Update () 
	{
        if (options.activeSelf)
            return;

        // Check if we're close enough to the destination
        if (Vector3.Distance(this.transform.position, destination_node.transform.position) < 0.1)// this.transform.position == destination_node.transform.position)// && !LevelManager.level_manager.level_settings.activeSelf)
        {
            float horizontal = 0;
            if (Mathf.Abs(Input.GetAxis("Horizontal")) >= Mathf.Abs(InputManager.ActiveDevice.LeftStickX.Value))
                horizontal = Input.GetAxis("Horizontal");
            else
                horizontal = InputManager.ActiveDevice.LeftStickX.Value;

            float vertical = 0;
            if (Mathf.Abs(Input.GetAxis("Vertical")) >= Mathf.Abs(InputManager.ActiveDevice.LeftStickY.Value))
                vertical = Input.GetAxis("Vertical");
            else
                vertical = InputManager.ActiveDevice.LeftStickY.Value;

            // Accept input if close enough
            if ((Mathf.Abs(vertical) > 0.1f || Mathf.Abs(horizontal) > 0.1f) && !LevelManager.level_manager.selected_level)
            {
                if (Mathf.Abs(vertical) > Mathf.Abs(horizontal))
                {
                    // Vertical
                    if (vertical > 0)
                    {
                        // Vertical up
                        if (destination_node.next_node.transform.position.y >= destination_node.previous_node.transform.position.y 
                            && Mathf.Abs(destination_node.next_node.transform.position.y - this.transform.position.y) > min_y_difference)
                        {
                            // Next node
                            SetNewDestination(destination_node.next_node);
                        }
                        else if (destination_node.previous_node.transform.position.y > destination_node.next_node.transform.position.y 
                            && Mathf.Abs(destination_node.previous_node.transform.position.y - this.transform.position.y) > min_y_difference)
                        {
                            // Previous node
                            SetNewDestination(destination_node.previous_node);
                        }
                    }
                    else if (vertical < 0)
                    {
                        // Vertical down
                        if (destination_node.next_node.transform.position.y <= destination_node.previous_node.transform.position.y 
                            && Mathf.Abs(destination_node.next_node.transform.position.y - this.transform.position.y) > min_y_difference)
                        {
                            // Next node
                            SetNewDestination(destination_node.next_node);
                        }
                        else if (destination_node.previous_node.transform.position.y < destination_node.next_node.transform.position.y 
                            && Mathf.Abs(destination_node.previous_node.transform.position.y - this.transform.position.y) > min_y_difference)
                        {
                            // Previous node
                            SetNewDestination(destination_node.previous_node);
                        }
                    }
                }
                else
                {
                    // Horizontal
                    if (horizontal > 0)
                    {
                        // Horziontal right
                        if (destination_node.next_node.transform.position.x >= destination_node.previous_node.transform.position.x 
                            && Mathf.Abs(destination_node.next_node.transform.position.x - this.transform.position.x) > min_x_difference)
                        {
                            // Next node
                            SetNewDestination(destination_node.next_node);
                        }
                        else if (destination_node.previous_node.transform.position.x > destination_node.next_node.transform.position.x 
                            && Mathf.Abs(destination_node.previous_node.transform.position.x - this.transform.position.x) > min_x_difference)
                        {
                            // Previous node
                            SetNewDestination(destination_node.previous_node);
                        }
                    }
                    else if (horizontal < 0)
                    {
                        // Horziontal left
                        if (destination_node.next_node.transform.position.x <= destination_node.previous_node.transform.position.x 
                            && Mathf.Abs(destination_node.next_node.transform.position.x - this.transform.position.x) > min_x_difference)
                        {
                            // Next node
                            SetNewDestination(destination_node.next_node);
                        }
                        else if (destination_node.previous_node.transform.position.x < destination_node.next_node.transform.position.x 
                            && Mathf.Abs(destination_node.previous_node.transform.position.x - this.transform.position.x) > min_x_difference)
                        {
                            // Previous node
                            SetNewDestination(destination_node.previous_node);
                        }
                    }
                }
            }
            else
            {
                destination_node.HoverOverLevel();

                if (options.activeSelf)
                    return;

                if (Input.GetButtonDown("Submit") || InputManager.ActiveDevice.Action1.WasPressed)
                {
                    destination_node.Button_Clicked();
                }
                if (Input.GetButtonDown("Cancel") || InputManager.ActiveDevice.Action2.WasPressed)
                {
                    if (LevelManager.level_manager.selected_level)
                    {
                        LevelManager.level_manager.DeselectLevels();
                    }
                    else
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("SetControllers");
                    }
                }
            }
        }
        else
        {
            // Move towards destination
            this.transform.position = Vector3.MoveTowards(this.transform.position, destination_node.transform.position, move_speed * Time.deltaTime);

            if (Vector3.Distance(destination_node.transform.position, transform.position) > 0.5f)
            {
                // Rotate towards it
                var dir = destination_node.transform.position - transform.position;
                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }
}
