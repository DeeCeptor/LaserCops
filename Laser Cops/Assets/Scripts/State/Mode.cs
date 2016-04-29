using UnityEngine;
using System.Collections;
using System;

public class Mode : MonoBehaviour
{
    public GameState.GameMode mode = GameState.GameMode.Cooperative;

    void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    public void SetGameMode(string input_mode)
    {
        mode = (GameState.GameMode) Enum.Parse(typeof(GameState.GameMode), input_mode, true);
        Debug.Log("Setting mode " + mode.ToString());
    }
}
