using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerJoin : MonoBehaviour
{
    public static PlayerJoin player_join;

    public int number_of_players = 2;
    public Text players_joined_text;
    public int num_controllers;

    public GameObject connected_controllers;
    public GridLayoutGroup controller_grid;
    public GameObject keyboard_column;

    public List<GameObject> player_icons;
    public List<GameObject> controller_icons;

    public BossHealthBarAnimation connect_more_controllers_circle;
    public GameObject connect_more_controllers;


    void Awake ()
    {
        player_join = this;
    }
    void Start ()
    {
        SetPlayerSettingsBasedOnPreviousPlay();
    }


    public void SetPlayerSettingsBasedOnPreviousPlay()
    {
        if (InputSettings.input_settings.inputs == null)
            return;

        number_of_players = InputSettings.input_settings.inputs.Count;

        // Turn on icons based on how many players there were
        int x = 0;
        foreach (List<string> l in InputSettings.input_settings.inputs)
        {
            // Players 1 and 2
            if (x < 2)
            {
                if (l.Count > 1)
                    player_icons[x].SetActive(true);
            }
            // Players 3 and 4
            else
            {
                if (l.Count > 0)
                    player_icons[x].SetActive(true);
            }
            x++;
        }
        Debug.Log("Set player input based on previous information");
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


        num_controllers = 0;
        foreach (string s in Input.GetJoystickNames())
        {
            if (!string.IsNullOrEmpty(s))
                num_controllers++;
        }

        if (num_controllers <= 0 && number_of_players > 2)
        {
            // Show warning telling to connect more controllers
            connect_more_controllers.SetActive(true);
            connect_more_controllers_circle.StartPulsing(Color.red);
        }
        else
            connect_more_controllers.SetActive(false);

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


        players_joined_text.text = "Players Joined:  " + number_of_players + "  controllers connected: " + num_controllers;
    }


    public List<List<string>> Finalize_Input()
    {
        List<List<string>> player_inputs = new List<List<string>>();
        for (int x = 0; x < number_of_players; x++)
        {
            player_inputs.Add(new List<string>());
        }

        // Add keyboard support to player 1 and 2
        player_inputs[0].Add("Keyboard Left");
        player_inputs[1].Add("Keyboard Right");

        int cont_1 = 0;
        int cont_2 = 0;
        int cont_3 = 0;
        int cont_4 = 0;

        // Check which controller (if any) does what
        foreach (GameObject p in player_icons)
        {
            if (p.activeSelf)
            {
                Debug.Log(p.name);
                // Determine what column they're in
                float x = p.transform.localPosition.x;
                if (x == 20f)
                {
                    if (cont_1 == 0)
                        player_inputs[Get_Player_Number_From_Name(p.name)].Add("Controller 1 Left");
                    else
                        player_inputs[Get_Player_Number_From_Name(p.name)].Add("Controller 1 Right");

                    cont_1++;
                }
                else if (x == 60f)
                {
                    if (cont_2 == 0)
                        player_inputs[Get_Player_Number_From_Name(p.name)].Add("Controller 2 Left");
                    else
                        player_inputs[Get_Player_Number_From_Name(p.name)].Add("Controller 2 Right");

                    cont_2++;
                }
                else if (x == 100f)
                {
                    if (cont_3 == 0)
                        player_inputs[Get_Player_Number_From_Name(p.name)].Add("Controller 3 Left");
                    else
                        player_inputs[Get_Player_Number_From_Name(p.name)].Add("Controller 3 Right");

                    cont_3++;
                }
                else if (x == 140f)
                {
                    if (cont_4 == 0)
                        player_inputs[Get_Player_Number_From_Name(p.name)].Add("Controller 4 Left");
                    else
                        player_inputs[Get_Player_Number_From_Name(p.name)].Add("Controller 4 Right");

                    cont_4++;
                }
                else
                {
                    Debug.Log(p.name + " is not in any column " + x);
                }
            }
        }

        // Print out inputs
        int player = 1;
        foreach (List<string> l in player_inputs)
        {
            Debug.Log("Player: " + player);
            foreach (string s in l)
            {
                Debug.Log(s);
            }
            player++;
        }

        return player_inputs;
    }

    
    public int Get_Player_Number_From_Name(string name)
    {
        if (name.Contains("P1"))
            return 0;
        else if (name.Contains("P2"))
            return 1;
        else if (name.Contains("P3"))
            return 2;
        else if (name.Contains("P4"))
            return 3;

        Debug.Log("Couldn't find player number from " + name);
        return 1;
    }
}
