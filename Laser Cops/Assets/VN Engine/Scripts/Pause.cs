using UnityEngine;
using System.Collections;

// Manages the paused state of the game
public class Pause : MonoBehaviour
{
    public static Pause pause;

    public GameObject pause_menu;   // Must be assigned for pausing to work
    public GameObject options_menu; // Must be assigned for options to be displayed

    public string toggle_pause_key = "Pause";

    [HideInInspector]
    public bool paused = false;

    void Awake ()
    {
        pause = this;
    }


    public void Toggle_Pause()
    {
        if (paused)
        {
            // Unpause
            pause_menu.SetActive(false);
            Time.timeScale = 1;
            paused = false;
            AudioListener.pause = false;
        }
        else
        {
            // Pause
            pause_menu.SetActive(true);
            Time.timeScale = 0;
            paused = true;
            AudioListener.pause = true;
        }
    }


    // Toggles the displaying of the options menu
    public void Toggle_Options()
    {
        options_menu.SetActive(!options_menu.activeSelf);
    }


    void Update ()
    {
        if (Input.GetButtonDown(toggle_pause_key))
            Toggle_Pause();
	}
}
