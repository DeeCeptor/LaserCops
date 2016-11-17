using UnityEngine;
using System.Collections;

public class PlayableLevelNode : LevelNode
{
    // Games modes allowed
    public bool coop = true;
    [HideInInspector]
    public string coop_diff;

    public bool competitive = true;
    [HideInInspector]
    public string competitive_diff;

    public bool chained = true;
    [HideInInspector]
    public string chained_diff;

    public bool no_tether = true;
    [HideInInspector]
    public string no_tether_diff;

    public bool tether_on = true;
    [HideInInspector]
    public string tether_on_diff;

    public bool one_hit_kill = true;
    [HideInInspector]
    public string one_hit_kill_diff;


    public GameObject beaten_grouping;
    string[] difficulties = { "Hard", "Normal", "Easy", "Unbeaten" }; 

    void Start()
    {
        // Load saved stats
        beat_level = System.Convert.ToBoolean(PlayerPrefs.GetInt(level_to_load, 0));
        if (!beat_level && required_to_beat)
            next_node = this;

        best_score = PlayerPrefs.GetInt(level_to_load + " High Score", 0);

        // Figure out what modes we've beaten and on what difficulty
        int beat_diffi = -1;
        if (coop)
        {
            beat_diffi = 3;
            for (int x = 0; x < difficulties.Length - 1; x++)
            {
                bool won = System.Convert.ToBoolean(PlayerPrefs.GetInt(level_to_load + " " + GameState.GameMode.Cooperative + " " + difficulties[x], 0));
                if (won)
                {
                    beat_diffi = x;
                    break;
                }
            }

            GameObject obj = GameObject.Instantiate(Resources.Load("Graphics/" + difficulties[beat_diffi]), Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.SetParent(beaten_grouping.transform);
            obj.transform.localScale = Vector3.one;
            coop_diff = difficulties[beat_diffi];
        }
        if (competitive && beat_level)
        {
            beat_diffi = 3;
            for (int x = 0; x < difficulties.Length - 1; x++)
            {
                bool won = System.Convert.ToBoolean(PlayerPrefs.GetInt(level_to_load + " " + GameState.GameMode.Competitive + " " + difficulties[x], 0));
                if (won)
                {
                    beat_diffi = x;
                    break;
                }
            }

            GameObject obj = GameObject.Instantiate(Resources.Load("Graphics/" + difficulties[beat_diffi]), Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.SetParent(beaten_grouping.transform);
            obj.transform.localScale = Vector3.one;
            competitive_diff = difficulties[beat_diffi];
        }
        if (chained && beat_level)
        {
            beat_diffi = 3;
            for (int x = 0; x < difficulties.Length - 1; x++)
            {
                bool won = System.Convert.ToBoolean(PlayerPrefs.GetInt(level_to_load + " " + GameState.GameMode.Chained + " " + difficulties[x], 0));
                if (won)
                {
                    beat_diffi = x;
                    break;
                }
            }

            GameObject obj = GameObject.Instantiate(Resources.Load("Graphics/" + difficulties[beat_diffi]), Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.SetParent(beaten_grouping.transform);
            obj.transform.localScale = Vector3.one;
            chained_diff = difficulties[beat_diffi];
        }
        if (no_tether && beat_level)
        {
            beat_diffi = 3;
            for (int x = 0; x < difficulties.Length - 1; x++)
            {
                bool won = System.Convert.ToBoolean(PlayerPrefs.GetInt(level_to_load + " " + GameState.GameMode.NoTether + " " + difficulties[x], 0));
                if (won)
                {
                    beat_diffi = x;
                    break;
                }
            }

            GameObject obj = GameObject.Instantiate(Resources.Load("Graphics/" + difficulties[beat_diffi]), Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.SetParent(beaten_grouping.transform);
            obj.transform.localScale = Vector3.one;
            no_tether_diff = difficulties[beat_diffi];
        }
        if (tether_on && beat_level)
        {
            beat_diffi = 3;
            for (int x = 0; x < difficulties.Length - 1; x++)
            {
                bool won = System.Convert.ToBoolean(PlayerPrefs.GetInt(level_to_load + " " + GameState.GameMode.TetherOn + " " + difficulties[x], 0));
                if (won)
                {
                    beat_diffi = x;
                    break;
                }
            }

            GameObject obj = GameObject.Instantiate(Resources.Load("Graphics/" + difficulties[beat_diffi]), Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.SetParent(beaten_grouping.transform);
            obj.transform.localScale = Vector3.one;
            tether_on_diff = difficulties[beat_diffi];
        }
        if (one_hit_kill && beat_level)
        {
            beat_diffi = 3;
            for (int x = 0; x < difficulties.Length - 1; x++)
            {
                bool won = System.Convert.ToBoolean(PlayerPrefs.GetInt(level_to_load + " " + GameState.GameMode.OneHitKill + " " + difficulties[x], 0));
                if (won)
                {
                    beat_diffi = x;
                    break;
                }
            }

            GameObject obj = GameObject.Instantiate(Resources.Load("Graphics/" + difficulties[beat_diffi]), Vector3.zero, Quaternion.identity) as GameObject;
            obj.transform.SetParent(beaten_grouping.transform);
            obj.transform.localScale = Vector3.one;
            one_hit_kill_diff = difficulties[beat_diffi];
        }
    }


    public override void LevelIsSelected()
    {
        base.LevelIsSelected();
        LevelManager.level_manager.high_score_text.text = "High Score: " + best_score;

        LevelManager.level_manager.coop_mode.SetActive(coop);

        LevelManager.level_manager.competitive_mode.SetActive(competitive && beat_level);
        LevelManager.level_manager.tether_on_mode.SetActive(tether_on && beat_level);
        LevelManager.level_manager.no_tether_mode.SetActive(no_tether && beat_level);
        LevelManager.level_manager.chained_mode.SetActive(chained && beat_level);
        LevelManager.level_manager.one_hit_kill_mode.SetActive(one_hit_kill && beat_level);
    }
}
