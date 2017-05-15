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
    public Text mode_description;

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

        switch (input_mode)
        {
            case "Cooperative":
                mode_description.text = "Work together with your friends to complete the level";
                break;
            case "Competitive":
                mode_description.text = "Be the last cop standing to claim victory!";
                break;
            case "NoTether":
                mode_description.text = "Cutbacks have caused your Lazer Tether to stop working";
                break;
            case "TetherOn":
                mode_description.text = "Due to cutbacks, the 'turn Lazer Tether off' button no longer works";
                break;
            case "Chained":
                mode_description.text = "For some reason your Lazer Tether is anchored to the center of the highway";
                break;
            case "OneHitKill":
                mode_description.text = "Take one hit, and you die. Same for goes the enemies, though";
                break;
        }
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
        //player_inputs = PlayerJoin.player_join.Finalize_Input();

        // Remove any players that have no inputs
        /*
        for (int x = 0; x < player_inputs.Count; x++)
        {
            if (player_inputs[x].Count <= 0)
            {
                player_inputs.RemoveAt(x);
                x--;
            }
        }

        InputSettings.input_settings.inputs = player_inputs;
        */

        Debug.Log("Loading level " + level_to_load);
        PlayerPrefs.SetString("LastLevelPlayed", level_to_load);
        UnityEngine.SceneManagement.SceneManager.LoadScene(level_to_load);
    }
}
