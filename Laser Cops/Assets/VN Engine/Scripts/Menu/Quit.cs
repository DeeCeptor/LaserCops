using UnityEngine;
using System.Collections;

public class Quit : MonoBehaviour
{
    public void Quit_Application()
    {
        Application.Quit(); 
    }


    public void To_Level_Select()
    {
        Time.timeScale = 1;
        
        UnityEngine.SceneManagement.SceneManager.LoadScene("SceneSelect");
    }


    public void Resume()
    {
        GameState.game_state.Unpause();
    }
}
