using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerCursor : MonoBehaviour 
{
    public LevelNode starting_node;
    public LevelNode hovering_over_level;
    public LevelNode destination_node;

    float min_y_difference = 1f;
    float min_x_difference = 1f;
    float move_speed = 4f;

	void Start () 
	{
        hovering_over_level = starting_node;
        this.transform.position = starting_node.transform.position;
        destination_node = starting_node;
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
        // Check if we're close enough to the destination
        if (this.transform.position == destination_node.transform.position)
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                if (Mathf.Abs(Input.GetAxis("Vertical")) > Mathf.Abs((Input.GetAxis("Horizontal"))))
                {
                    // Vertical
                    if (Input.GetAxis("Vertical") > 0)
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
                    else if (Input.GetAxis("Vertical") < 0)
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
                    if (Input.GetAxis("Horizontal") > 0)
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
                    else if (Input.GetAxis("Horizontal") < 0)
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

                if (Input.GetButtonDown("Submit"))
                {
                    destination_node.Button_Clicked();
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