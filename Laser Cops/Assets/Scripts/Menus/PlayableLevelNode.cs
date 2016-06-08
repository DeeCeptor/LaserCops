using UnityEngine;
using System.Collections;

public class PlayableLevelNode : LevelNode
{
    // Games modes allowed
    public bool coop = true;
    public bool competitive = true;
    public bool chained = true;
    public bool no_tether = true;
    public bool tether_on = true;
    public bool one_hit_kill = true;

    void Start()
    {
        // Load saved stats
        beat_level = System.Convert.ToBoolean(PlayerPrefs.GetInt(level_to_load, 0));
        if (!beat_level && !required_to_beat)
            next_node = this;

        best_score = PlayerPrefs.GetInt(level_to_load + " High Score", 0);
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
