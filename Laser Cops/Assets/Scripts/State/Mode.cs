using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class Mode : MonoBehaviour
{
    public GameState.GameMode mode = GameState.GameMode.Cooperative;
    public string level_to_load;
    public Toggle object_to_select_after_clicking;
    public GameObject settings_menu;

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

        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    public void SetGameMode(string input_mode)
    {
        mode = (GameState.GameMode) Enum.Parse(typeof(GameState.GameMode), input_mode, true);
        Debug.Log("Setting mode " + mode.ToString());
    }
    public void SetLevelToLoad(string level_name)
    {
        level_to_load = level_name;

        if (!object_to_select_after_clicking)
        {
            settings_menu.SetActive(true);
            object_to_select_after_clicking.Select();
        }
    }

    public void Load_Level()
    {
        Debug.Log("Loading level " + level_to_load);
        SceneManager.LoadScene(level_to_load);
    }
}
