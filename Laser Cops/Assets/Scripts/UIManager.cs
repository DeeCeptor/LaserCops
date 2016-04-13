using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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

    public Text announcement_text;
    [HideInInspector]
    public Queue<string> announcements = new Queue<string>();
    float cur_announcement_cooldown;
    float announcement_cooldown = 1f;

    void Awake ()
    {
        ui_manager = this;
    }
	void Start ()
    {
        UpdateHealth();
        UpdateScore();

        SetAnnouncementText(new string[] { "3", "2", "1", "Go!" });
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


    public void SetAnnouncementText(string announcement)
    {
        announcements.Enqueue(announcement);
    }
    public void SetAnnouncementText(string[] new_announcements)
    {
        foreach (string s in new_announcements)
        {
            announcements.Enqueue(s);
        }
    }


    void LateUpdate ()
    {
        // Update the time
        time_text.text = time_string + GameState.game_state.getFormattedTime(GameState.game_state.elapsed_game_time);

        // Update announcement text
        cur_announcement_cooldown -= Time.deltaTime;
        if (cur_announcement_cooldown <= 0)
        {
            if (announcements.Count > 0)
            {
                // Switch text
                announcement_text.text = announcements.Dequeue();
                cur_announcement_cooldown = announcement_cooldown;
            }
            else if (announcement_text.text != "")
            {
                announcement_text.text = "";
            }
        }
	}
}
