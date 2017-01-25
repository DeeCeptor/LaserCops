using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager ui_manager;

    public GameObject pause_menu;

    public Image player_1_health;
    public float player_1_health_starting_width;
    public Image player_2_health;
    public float player_2_health_starting_width;
    public Image player_3_health;
    public float player_3_health_starting_width;
    public Image player_4_health;
    public float player_4_health_starting_width;

    public Text score_text;
    [HideInInspector]
    public string score_string = "Score: ";
    [HideInInspector]
    public int score;
    float multiplier;   // Multiplier we are at
    public Slider multiplier_slider;
    public Text multiplierText;
    public int score_for_a_link = 20;
    public int previous_link_score = 0;
    public int score_needed_for_new_link = 20;

    public Text time_text;
    [HideInInspector]
    private string time_string = "";//"Time: ";

    public Slider bottom_health_bar;
    public Image bottom_health_bar_color;
    public Text bottom_text_name;
    public GameObject bottom_bar;
    float target_of_bottom_bar;
    float cur_bottom_bar;
    public BossHealthBarAnimation boss_hp_anim;

    public Text announcement_text;
    [HideInInspector]
    public Queue<string> announcements = new Queue<string>();
    float cur_announcement_cooldown;
    float announcement_cooldown = 1f;

    public GameObject end_of_level_text;


    void Awake ()
    {
        ui_manager = this;

        time_text = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();
        score_text = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        player_1_health = GameObject.FindGameObjectWithTag("1HP").GetComponent<Image>();
        player_1_health_starting_width = player_1_health.rectTransform.rect.width;
        player_2_health = GameObject.FindGameObjectWithTag("2HP").GetComponent<Image>();
        player_2_health_starting_width = player_2_health.rectTransform.rect.width;
        player_3_health = GameObject.FindGameObjectWithTag("3HP").GetComponent<Image>();
        player_3_health_starting_width = player_3_health.rectTransform.rect.width;
        player_4_health = GameObject.FindGameObjectWithTag("4HP").GetComponent<Image>();
        player_4_health_starting_width = player_4_health.rectTransform.rect.width;
    }
    void Start ()
    {
        UpdateHealth();
        UpdateScore();

        if (GameState.game_state.number_of_players < 4)
        {
            player_4_health.gameObject.SetActive(false);
        }
        if (GameState.game_state.number_of_players < 3)
        {
            player_3_health.gameObject.SetActive(false);
        }
        //SetAnnouncementText(new string[] { "3", "2", "1", "Go!" });
    }

    public void UpdateHealth()
    {
        foreach (PlayerController player in GameState.game_state.Players)
        {
            int number_of_ticks = ((int)((player.Health / player.Max_Health) / 0.04f));
            float size = number_of_ticks * 0.04f;

            if (player.player_number == 1)
            {
                player_1_health.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size * player_1_health_starting_width);
                //player_1_health.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (player.Health / (player.Max_Health)) * player_1_health_starting_width);
            }
            else if (player.player_number == 2)
            {
                player_2_health.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size * player_2_health_starting_width);
                //player_2_health.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (player.Health / (player.Max_Health)) * player_2_health_starting_width);
            }
            else if (player.player_number == 3)
            {
                player_3_health.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size * player_3_health_starting_width);
                //player_2_health.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (player.Health / (player.Max_Health)) * player_2_health_starting_width);
            }
            else if (player.player_number == 4)
            {
                player_4_health.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size * player_4_health_starting_width);
                //player_2_health.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (player.Health / (player.Max_Health)) * player_2_health_starting_width);
            }
        }
    }


    public void ChangeScore(int amount)
    {
        ChangeScore(amount, Vector3.zero);
    }
    public void ChangeScore(int amount, Vector3 position)
    {
        score += amount;
        score = Mathf.Max(0, score);    // Score can't go below 0

        if (position != Vector3.zero)
            EffectsManager.effects.spawnMovingText(position, "+" + amount);

        multiplier_slider.value += ((float)amount) / (multiplier * 100);
        if (multiplier_slider.value >= 1)
            addMultiplierLevel();

        // Add new links if we reached the necessary score
        if (Tether.tether != null && score >= score_needed_for_new_link)
        {
            Tether.tether.AddLink();
            previous_link_score = score;
            score_for_a_link += 3;
            score_needed_for_new_link = score + score_for_a_link;
        }

        UpdateScore();
    }
    public void UpdateScore()
    {
        score_text.text = score_string + score;
    }


    public void ActivateBottomHealthBar(string boss_name, Color color, float max_health)
    {
        bottom_text_name.text = boss_name;
        bottom_text_name.GetComponent<Outline>().effectColor = new Color(color.r, color.g, color.b, 0.5f);
        bottom_health_bar.minValue = 0;
        bottom_health_bar.maxValue = max_health;
        target_of_bottom_bar = max_health;
        bottom_health_bar.value = 0;
        bottom_health_bar_color.color = color;
        bottom_bar.SetActive(true);

        Debug.Log("Activating bottom bar max hp: " + max_health);
    }
    public void UpdateBottomHealthBar(float cur_health)
    {
        if (cur_health < target_of_bottom_bar)
        {
            // Losing HP
            boss_hp_anim.StartPulsing(Color.red);
        }
        else
        {
            // Gaining HP
            boss_hp_anim.StartPulsing(Color.green);
        }

        target_of_bottom_bar = cur_health;
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
        //EffectsManager.effects.spawnMovingText(pos, "+0.2!", Vector3.up * 2, 40).transform.SetParent(Camera.main.transform);
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
        if (Tether.tether!=null)
        {
            multiplierText.text = "X" + multiplier.ToString("0.00" + " Length: " + Tether.tether.tether_links.Count);
        }
    }


    void Update ()
    {
        // Adjust score multiplier
        // Update cleaniplier
        multiplier_slider.value -= Time.deltaTime / 10;
        if (multiplier_slider.value <= 0)
            lowerMultiplierLevel();

        // Smoothing boss HP
        if (bottom_bar.activeSelf)
        {
            //bottom_health_bar.value = (int) (( Mathf.Lerp(bottom_health_bar.value, target_of_bottom_bar, Time.deltaTime * 0.5f) / bottom_health_bar.maxValue) * 40);
            cur_bottom_bar = Mathf.MoveTowards(cur_bottom_bar, target_of_bottom_bar, Time.deltaTime * bottom_health_bar.maxValue / 5f);
            int number_of_ticks = (int) (cur_bottom_bar / (float) bottom_health_bar.maxValue / 0.025f) + 1;
            bottom_health_bar.value = (float) number_of_ticks / 40f * bottom_health_bar.maxValue;
            /*
            bottom_health_bar.value = 
                (
                    (
                        (Mathf.MoveTowards(bottom_health_bar.value, target_of_bottom_bar, Time.deltaTime * bottom_health_bar.maxValue / 5f)
                        / bottom_health_bar.maxValue / 0.025f)
                    )  * 0.025f)
                * bottom_health_bar.maxValue;*/
        }
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
