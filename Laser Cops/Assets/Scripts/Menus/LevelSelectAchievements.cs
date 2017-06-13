using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectAchievements : MonoBehaviour 
{
    // Used for Easy Does It achievement
    public List<LevelNode> tutorial_levels = new List<LevelNode>();


	void Start () 
	{
        StartCoroutine(CheckForAchievements());
	}


    IEnumerator CheckForAchievements()
    {
        yield return new WaitForSeconds(1f);

        // Check for Easy does it (first 2 levels)
        bool beat_tutorial_levels = true;
        foreach (LevelNode node in tutorial_levels)
        {
            if (!node.beat_level)
            {
                beat_tutorial_levels = false;
                break;
            }
        }
        if (beat_tutorial_levels)
            GetAchievement("Easy Does It");

        // Loop through each level, check if they were completed on hard mode
        bool all_on_hard = true;
        foreach (LevelNode node in LevelManager.level_manager.level_nodes)
        {
            if (!node.beat_level || !node.GetComponent<PlayableLevelNode>().beat_any_mode_on_hard) //node.diff)
            {
                all_on_hard = false;
                break;
            }
        }
        if (all_on_hard)
        {
            Debug.Log("ALL LEVELS COMPLETE ON HARD");
            GetAchievement("Completionist");
        }
    }


    public void GetAchievement(string name)
    {
        if (SteamManager.Initialized)
        {
            try
            {
                bool successful = SteamUserStats.SetAchievement(name);
                if (successful)
                    Debug.Log("Got achievement: " + name);
                else
                    Debug.LogError("Did not get achievement: " + name);

                successful = SteamUserStats.StoreStats();
                if (successful)
                    Debug.Log("Stored stats after achievement: " + name);
                else
                    Debug.LogError("Did not store stats achievement: " + name);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message, this);
            }
        }
        else
            Debug.Log("SteamManager not initialized or achievement_name is empty");
    }
}
