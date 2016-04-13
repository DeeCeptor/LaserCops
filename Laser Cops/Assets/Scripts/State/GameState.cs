using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameState : MonoBehaviour
{
    public static GameState game_state;

    public string level_to_load_on_victory = "LevelSelect";
    public string level_to_load_on_defeat = "LevelSelect";

    public bool paused = false; // Paused by player
    public bool game_over = false;  // Lost the game
    public float elapsed_game_time = 0f;
    public bool going_sideways;

    public float inactive_speed = 1.0f;     // How quickly objects move when offscreen

    public List<PlayerController> Players = new List<PlayerController>();
    public GameObject[] PlayerObjects;
    public bool VIP = false;
    public GameObject VIPObject;

    public bool tether_touching_obstacle = false;
    public float time_last_touched_obstacle;
    float turn_off_tether_touching_obstacle_time = 0.3f;

    void Awake ()
    {
        game_state = this;
        SetGameSettings();
        PlayerObjects = GameObject.FindGameObjectsWithTag("Player");
        if (VIP)
        {
            VIPObject = GameObject.FindGameObjectWithTag("VIP");
        }
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



    void Update()
    {
        if (Input.GetButtonDown("Pause") && !game_over)
        {
            if (Time.timeScale == 0)
            {
                // Currently paused, unpause game
                Unpause();
            }
            else
            {
                // Currently unpaused, pause game
                Pause();
            }
        }

        // Not paused and not game over
        if (Time.timeScale != 0 && !game_over)
        {
            // Timer is running if we're not paused
            elapsed_game_time += Time.deltaTime;
        }


        // Is the tether touching an obstacle?
        if (tether_touching_obstacle && 
            time_last_touched_obstacle + turn_off_tether_touching_obstacle_time < Time.time)
        {
            tether_touching_obstacle = false;
        }
    }


    public string getFormattedTime(float in_time)
    {
        int minutes = (int)((in_time) / 60.0f);
        int seconds = (int)(in_time % 60);
        int milliseconds = (int)((in_time - (minutes * 60) - seconds) * 100);
        return ("" + minutes).PadLeft(2, '0') + ":" + ("" + seconds).PadLeft(2, '0') + "." + ("" + milliseconds).PadLeft(2, '0');
    }


    public void GameOver()
    {
        if (!game_over)
        {
            Debug.Log("You lose!");
            game_over = true;
            ChangeScene(3f, level_to_load_on_defeat);
        }
    }
    public void Victory()
    {
        if (!game_over)
        {
            Debug.Log("You won the level!");
            game_over = true;
            ChangeScene(3f, level_to_load_on_victory);
        }
    }


    public void ChangeScene(float delay, string scene_to_load)
    {
        Time.timeScale = 1;
        StartCoroutine(loadMenu(delay, scene_to_load));
    }
    public IEnumerator loadMenu(float delay, string scene_to_load)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(scene_to_load);
        yield return null;
    }


    public void Unpause()
    {
        Time.timeScale = 1;
        //PauseMenu.SetActive(false);
    }
    public void Pause()
    {
        Time.timeScale = 0;
        //PauseMenu.SetActive(true);
    }
}
