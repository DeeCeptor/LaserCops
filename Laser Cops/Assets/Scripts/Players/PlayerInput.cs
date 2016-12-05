using UnityEngine;
using System.Collections.Generic;

// Base object that takes all necessary player input
public class PlayerInput : MonoBehaviour
{
    public List<string> inputs_to_check;
    public int player_number;   // 1 = left car, 2 = right car, 3, 4
    public Vector2 direction = new Vector2();
    public bool tether_switched = false;
    public bool tether_held_down = false;
    public bool tether_released_this_instant = false;
    public bool boosted_this_instant = false;
    public bool disable_tether_held_down = false;

    // Doesn't get call unless base.Start is called
    void Start ()
    {

    }

    // Doesn't get call unless base.Update is called
    void Update ()
    {

    }


    public float GetAxisUsingInputList(string axis)
    {
        float highest = 0;
        foreach (string input in inputs_to_check)
        {
            float cur = Input.GetAxis(input + " " + axis);

            if (Mathf.Abs(cur) > Mathf.Abs(highest))
                highest = cur;
        }
        return highest;
    }
    public bool GetButtonUsingInputList(string button)
    {
        bool pressed = false;
        foreach (string input in inputs_to_check)
        {
            bool cur = Input.GetButtonDown(input + " " + button);
            pressed = pressed || cur;
        }
        return pressed;
    }


    public void UpdateInputs()
    {
        if (GameState.game_state.going_sideways)
        {
            direction.x = GetAxisUsingInputList("X Steering");
            direction.y = GetAxisUsingInputList("Y Steering");
        }
        else
        {
            direction.x = GetAxisUsingInputList("Y Steering");
            direction.y = GetAxisUsingInputList("X Steering");
        }
        boosted_this_instant = GetButtonUsingInputList("Boost");


        tether_switched = Input.GetButtonDown("Switch Tether " + player_number);
        tether_held_down = Input.GetButton("Switch Tether " + player_number);
        tether_released_this_instant = Input.GetButtonUp("Switch Tether " + player_number);

        if (Input.GetButton("Disable Tether") || Input.GetAxisRaw("Disable Tether") != 0)
        {
            disable_tether_held_down = true;
        }
        else
            disable_tether_held_down = false;



        /* OLD
        if (GameState.game_state.going_sideways)
        {
            direction.x = Input.GetAxis("Player " + player_number + " X Steering");
            direction.y = Input.GetAxis("Player " + player_number + " Y Steering");
        }
        else
        {
            direction.x = Input.GetAxis("Player " + player_number + " Y Steering");
            direction.y = Input.GetAxis("Player " + player_number + " X Steering");
        }

        tether_switched = Input.GetButtonDown("Switch Tether " + player_number);
        tether_held_down = Input.GetButton("Switch Tether " + player_number);
        tether_released_this_instant = Input.GetButtonUp("Switch Tether " + player_number);

        boosted_this_instant = Input.GetButtonDown("Player " + player_number + " Boost");

        if (Input.GetButton("Disable Tether") || Input.GetAxisRaw("Disable Tether") != 0)
        {
            disable_tether_held_down = true;
        }
        else
            disable_tether_held_down = false;
        */
    }


    public void Disabled_Input()
    {
        direction = Vector2.zero;
        disable_tether_held_down = false;
        boosted_this_instant = false;
        tether_switched = false;
        tether_held_down = false;
    }
}
