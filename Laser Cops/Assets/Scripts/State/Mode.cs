using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Collections;

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
        Debug.Log("Loading level " + level_to_load);
        PlayerPrefs.SetString("LastLevelPlayed", level_to_load);

        StartCoroutine(Async_Load_Level());
    }


    IEnumerator Async_Load_Level()
    {
        string active_scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        Destroy(UnityEngine.EventSystems.EventSystem.current.gameObject);

        AsyncOperation AO = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(level_to_load, UnityEngine.SceneManagement.LoadSceneMode.Single);
        AO.allowSceneActivation = false;

        while (AO.progress < 0.9f)
        {
            yield return null;
        }
        AO.allowSceneActivation = true;
        //UnityEngine.SceneManagement.SceneManager.un(active_scene);
    }
    /*
        IEnumerator Async_Load_Level()
    {
        UIManager.ui_manager.loading_icon.SetActive(true);
        UIManager.ui_manager.loading_text.gameObject.SetActive(true);
        string active_scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        DestroyImmediate(UnityEngine.EventSystems.EventSystem.current.gameObject);

        AsyncOperation AO = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(level_to_load, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        AO.allowSceneActivation = false;
        int progress = (int)(AO.progress * 100f);
        while (AO.progress < 0.9f)
        {
            progress = Mathf.Max(progress, (int)(AO.progress * 100f));
            UIManager.ui_manager.loading_text.text = "Loading... " + progress + "%";
            yield return null;
        }
        AO.allowSceneActivation = true;
        while (AO.progress < 1f)
        {
            progress = Mathf.Max(progress, (int)(AO.progress * 100f));
            UIManager.ui_manager.loading_text.text = "Loading... " + progress + "%";
            yield return null;
        }

        yield return 0;

        UIManager.ui_manager.loading_icon.SetActive(false);
        UIManager.ui_manager.loading_text.gameObject.SetActive(false);

        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(active_scene);
        Debug.Log("Done Async loading & switching levels");
    }
    */
}
