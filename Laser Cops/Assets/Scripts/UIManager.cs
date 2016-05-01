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
    float multiplier;   // Multiplier we are at
    public Slider multiplier_slider;
    public Text multiplierText;

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
                player_1_health.value = player.Health / player.Max_Health * 100;
            }
            else if (player.player_number == 2)
            {
                player_2_health.value = player.Health / player.Max_Health * 100;
            }
        }
    }


    public void ChangeScore(int amount)
    {
        ChangeScore(amount, Vector3.zero);
    }
    public void ChangeScore(int amount, Vector3 position)
    {
        Tether.tether.AddLink();

        score += amount;
        score = Mathf.Max(0, score);    // Score can't go below 0

        if (position != Vector3.zero)
            EffectsManager.effects.spawnMovingText(position, "+" + amount);

        multiplier_slider.value += ((float)amount) / (multiplier * 100);
        if (multiplier_slider.value >= 1)
            addMultiplierLevel();

        UpdateScore();
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
        Debug.Log(cur_health);
        bottom_health_bar.value = cur_health;
    }


    public void SetAnnouncementText(string announcement, float length_of_announcement = 1f)
    {
        announcement_cooldown = length_of_announcement;
        announcements.Enqueue(announcement);
    }
    public void SetAnnouncementText(string[] new_announcements, float length_of_announcement = 1f)
    {
        foreach (string s in new_announcements)
        {
            announcements.Enqueue(s);
        }
        announcement_cooldown = length_of_announcement;
    }


    public void addMultiplierLevel()
    {
        multiplier_slider.value = 0.4f;
        multiplier += 0.2f;
        setMultiplierText();
        Vector3 pos = new Vector3(
            Camera.main.ScreenToWorldPoint(multiplierText.transform.position).x,
            Camera.main.ScreenToWorldPoint(multiplierText.transform.position).y,
            0);
        EffectsManager.effects.spawnMovingText(pos, "+0.2!", Vector3.up * 2, 40).transform.SetParent(Camera.main.transform);
        /*
        if (multiplier >= nextMultiplierSound && multiplierSounds.Count > 0)
        {
            // Play sound, increase amount we need for next multiplier sound
            nextMultiplierSound++;
            AudioSource.PlayClipAtPoint(multiplierSounds[0], Camera.main.transform.position);
            multiplierSounds.RemoveAt(0);   // Remove the sound so we don't hear it again

            // Show announcement text on screen
            Vector3 posi = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, -1));
            posi.z = -1;
            string text = multiplierAnnouncements[0];
            multiplierAnnouncements.RemoveAt(0);
            spawnMovingText(posi, text, Vector3.down * 10, 80).transform.SetParent(Camera.main.transform);
        }*/
    }
    public void lowerMultiplierLevel()
    {
        if (multiplier > 1.0f)
        {
            // Lower level if we can
            multiplier_slider.value = 0.9f;
            multiplier -= 0.2f;
            setMultiplierText();
        }
    }
    public void setMultiplierText()
    {
        multiplierText.text = "X" + multiplier.ToString("0.00" + " Length: " + Tether.tether.tether_links.Count);
    }


    void Update ()
    {
        // Adjust score multiplier
        // Update cleaniplier
        multiplier_slider.value -= Time.deltaTime / 10;
        if (multiplier_slider.value <= 0)
            lowerMultiplierLevel();
    }


    void LateUpdate ()
    {
        // Update the time
        time_text.text = time_string + GameState.game_state.getFormattedTime(GameState.game_state.elapsed_game_time);
        UpdateScore();
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
