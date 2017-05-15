using UnityEngine;
using System.Collections.Generic;
using InControl;

// Base object that takes all necessary player input
public class PlayerInput : MonoBehaviour
{
    public List<string> inputs_to_check;

    // InControl device setup
    public InputDevice controller;
    public bool left_side_of_controller = true;

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
        // Don't get input if paused
        if (Time.timeScale == 0)
            return;

        // If player 1 or 2, use keyboard as well

        // No controller detected
        if (controller == null)
        {
            Disabled_Input();
            return;
        }

        string side_to_use = "Left";
        if (!left_side_of_controller)
            side_to_use = "Right";

        if (GameState.game_state.going_sideways)
        {
            direction.x = controller.GetControlByName(side_to_use + "StickX").Value;
            direction.y = controller.GetControlByName(side_to_use + "StickY").Value;
        }
        else
        {
            direction.x = controller.GetControlByName(side_to_use + "StickY").Value;
            direction.y = controller.GetControlByName(side_to_use + "StickX").Value;
        }

        if (left_side_of_controller)
            boosted_this_instant = controller.GetControlByName(side_to_use + "StickButton").Value == 1 ? true : false;
        else
            boosted_this_instant = controller.Action1.WasPressed ? true : controller.GetControlByName(side_to_use + "StickButton").Value == 1 ? true : false;

        tether_switched = controller.GetControlByName(side_to_use + "Bumper").WasPressed;

        disable_tether_held_down = controller.GetControlByName(side_to_use + "Trigger").Value != 0;
        /*
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
