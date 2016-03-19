using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour
{
    public static GameState game_state;

    public bool paused = false; // Paused by player
    public bool game_over = false;  // Lost the game

    public bool going_sideways = true;

    void Awake ()
    {
        game_state = this;
        SetGameSettings();
    }
	void Start ()
    {
	
	}


    public void SetGameSettings()
    {
        // Vastly improves frame rate, making stars scrolling by not look terrible
        QualitySettings.vSyncCount = 0;

        // Set the correct gravity
        SetGravity(new Vector3(-9.81f, 0));
    }
    public void SetGravity(Vector2 new_gravity)
    {
        Physics2D.gravity = new_gravity;
        Physics.gravity = new_gravity;

        if (Mathf.Abs(new_gravity.x) >= Mathf.Abs(new_gravity.y))
            going_sideways = true;
        else
            going_sideways = false;
    }


    void Update ()
    {

    }
}
