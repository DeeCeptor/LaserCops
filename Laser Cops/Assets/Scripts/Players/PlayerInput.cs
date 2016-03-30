using UnityEngine;
using System.Collections;

// Base object that takes all necessary player input
public class PlayerInput : MonoBehaviour
{
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

    public void UpdateInputs()
    {
        direction.x = Input.GetAxis("Player " + player_number + " X Steering");
        direction.y = Input.GetAxis("Player " + player_number + " Y Steering");

        tether_switched = Input.GetButtonDown("Switch Tether " + player_number);
        tether_held_down = Input.GetButton("Switch Tether " + player_number);
        tether_released_this_instant = Input.GetButtonUp("Switch Tether " + player_number);

        boosted_this_instant = Input.GetButtonDown("Player " + player_number + " Boost");

        disable_tether_held_down = Input.GetButton("Disable Tether");
    }
}
