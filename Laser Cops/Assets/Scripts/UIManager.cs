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

    public Slider bottom_health_bar;
    public Image bottom_health_bar_background;
    public Text bottom_text_name;
    public GameObject bottom_bar;

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


    public void ActivateBottomHealthBar(string boss_name, Color color, float max_health)
    {
        bottom_text_name.text = boss_name;
        bottom_health_bar_background.color = color;
        bottom_health_bar.minValue = 0;
        bottom_health_bar.maxValue = max_health;

        bottom_bar.SetActive(true);
    }
    public void UpdateBottomHealthBar(float cur_health)
    {
        bottom_health_bar.value = cur_health;
    }


    void LateUpdate ()
    {
        // Update the time
        time_text.text = time_string + GameState.game_state.getFormattedTime(GameState.game_state.elapsed_game_time);
	}
}
