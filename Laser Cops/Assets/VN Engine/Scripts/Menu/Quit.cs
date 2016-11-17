using UnityEngine;
using System.Collections;

public class Quit : MonoBehaviour
{
    public void Resume()
    {
        GameState.game_state.Unpause();
    }


    public void To_Level_Select()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("SceneSelect");

    }


    public void Clear_Preferences()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Preferences cleared");
    }


    public void Quit_Application()
    {
        Application.Quit(); 
    }
}
