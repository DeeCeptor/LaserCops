using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager ui_manager;

    public Slider player_1_health;
    public Slider player_2_health;

    public Text score_text;
    [HideInInspector]
    public string score_string = "Score: ";
    [HideInInspector]
    public int score;

    public Text time_text;
    [HideInInspector]
    public string time_string = "Time: ";

    void Awake ()
    {
        ui_manager = this;
    }
	void Start ()
    {
        UpdateHealth();
        UpdateScore();
	}

    public void UpdateHealth()
    {
        foreach (PlayerController player in GameState.game_state.Players)
        {
            if (player.player_number == 1)
            {
                player_1_health.value = player.Health;
            }
            else if (player.player_number == 2)
            {
                player_2_health.value = player.Health;
            }
        }
    }


    public void ChangeScore(int amount)
    {
        score += amount;
    }
    public void UpdateScore()
    {
        score_text.text = score_string + score;
    }

    void LateUpdate ()
    {
        // Update the time
        time_text.text = time_string + GameState.game_state.getFormattedTime(GameState.game_state.elapsed_game_time);
	}
}
