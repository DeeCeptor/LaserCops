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

    public override void LevelIsSelected()
    {
        base.LevelIsSelected();

        LevelManager.level_manager.coop_mode.SetActive(coop);
        LevelManager.level_manager.competitive_mode.SetActive(competitive);
        LevelManager.level_manager.tether_on_mode.SetActive(tether_on);
        LevelManager.level_manager.no_tether_mode.SetActive(no_tether);
        LevelManager.level_manager.one_hit_kill_mode.SetActive(one_hit_kill);
        LevelManager.level_manager.chained_mode.SetActive(chained);

    }
}
