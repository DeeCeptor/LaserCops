using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class Mode : MonoBehaviour
{
    public static Mode current_mode;
    public GameState.GameMode mode = GameState.GameMode.Cooperative;
    public GameState.Difficulty difficulty = GameState.Difficulty.Normal;   // Current difficulty, passed into the level
    public string level_to_load;
    public Toggle object_to_select_after_clicking;
    public GameObject settings_menu;

    public List<List<string>> player_inputs;

    void Awake()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("GameMode"))
        {
            if (obj != this.gameObject)
            {
                Debug.Log("Removing duplicate game settings for mode " + mode.ToString());
                Destroy(this.gameObject);
                return;
            }
        }

        current_mode = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    public void SetGameMode(string input_mode)
    {
        mode = (GameState.GameMode) Enum.Parse(typeof(GameState.GameMode), input_mode, true);
        Debug.Log("Setting mode " + mode.ToString());
    }
    public void SetDifficulty(string input_mode)
    {
        difficulty = (GameState.Difficulty)Enum.Parse(typeof(GameState.Difficulty), input_mode, true);
    }
    public void SetLevelToLoad(string level_name)
    {
        level_to_load = level_name;
    }

    public void Load_Level()
    {
        // Get the input settings of each player
        player_inputs = PlayerJoin.player_join.Finalize_Input();

        Debug.Log("Loading level " + level_to_load);
        PlayerPrefs.SetString("LastLevelPlayed", level_to_load);
        UnityEngine.SceneManagement.SceneManager.LoadScene(level_to_load);
    }
}
