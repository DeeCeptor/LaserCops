using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerJoin : MonoBehaviour
{
    public int number_of_players = 2;
    public Text players_joined_text;

    public GameObject connected_controllers;
    public GridLayoutGroup controller_grid;
    public GameObject keyboard_column;

    public List<GameObject> player_icons;
    public List<GameObject> controller_icons;

    void Start ()
    {
	
	}


    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            number_of_players = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            number_of_players = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            number_of_players = 4;
        }
        else if (Input.GetButtonDown("Switch Tether 1"))
        {
            number_of_players++;
            if (number_of_players > 4)
                number_of_players = 2;
        }


        int num_controllers = 0;
        foreach (string s in Input.GetJoystickNames())
        {
            if (!string.IsNullOrEmpty(s))
                num_controllers++;
        }

        int unallocated_players = number_of_players;

        for (int x = 4; x > 0; x--)
        {
            // Show/hide controller icons based on connected controllers
            if (num_controllers >= x)
            {
                // Show controller
                controller_icons[x - 1].SetActive(true);
            }
            else
            {
                // Hide controller
                controller_icons[x - 1].SetActive(false);
            }

            // Show/hide player
            if (number_of_players >= x)
            {
                // Show player
                player_icons[x - 1].SetActive(true);
            }
            else
            {
                // Hide player
                player_icons[x - 1].SetActive(false);
            }



            // Fill the controllers as much as possible

            // No controllers left, use keyboard column
        }

        controller_grid.constraintCount = num_controllers;



        // Check if we have more players than controllers
        if (number_of_players > num_controllers * 2)
        {
            // 3 or 4 players, eliminate player 1 or 2 from controllers
            // Remove player 1 or player 2 from controllers
            int remove = Mathf.Abs(number_of_players - num_controllers * 2);
            for (int x = 0; x < remove; x++)
            {
                player_icons[x].SetActive(false);
            }
        }



        players_joined_text.text = "Players joined:  " + number_of_players + "  controllers connected: " + num_controllers;
    }
}
