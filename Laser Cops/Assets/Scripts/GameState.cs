using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState : MonoBehaviour
{
    public static GameState game_state;

    public bool paused = false; // Paused by player
    public bool game_over = false;  // Lost the game
    public float elapsed_game_time = 0f;
    public bool going_sideways = true;

    public List<PlayerController> Players = new List<PlayerController>();
    public GameObject[] PlayerObjects;
    public bool VIP = false;
    public GameObject VIPObject;

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
    }


    public string getFormattedTime(float in_time)
    {
        int minutes = (int)((in_time) / 60.0f);
        int seconds = (int)(in_time % 60);
        int milliseconds = (int)((in_time - (minutes * 60) - seconds) * 100);
        return ("" + minutes).PadLeft(2, '0') + ":" + ("" + seconds).PadLeft(2, '0') + "." + ("" + milliseconds).PadLeft(2, '0');
    }


    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        StartCoroutine(loadMenu());
    }
    public IEnumerator loadMenu()
    {
        /*
		GameObject[] objs = GameObject.FindObjectsOfType<GameObject>();
		Debug.Log(objs.Length);
		for (int x = 0; x < objs.Length; x++) {
			GameObject.DestroyImmediate(objs[x]);
		}*/
        Application.LoadLevel("Menu");
        yield return new WaitForSeconds(0f);
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
