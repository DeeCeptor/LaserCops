using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class LevelManager : MonoBehaviour 
{
    public static LevelManager level_manager;
    List<GameObject> levels = new List<GameObject>();
    LineRenderer line;
    public LineRenderer line_2;
    LevelNode[] level_nodes;
    Mode game_mode;
    public Text high_score_text;

    public GameObject selected_level_ui;
    public Text level_text_name;
    public GameObject level_settings;
    public GameObject object_to_select;

    public GameObject coop_mode;
    public GameObject competitive_mode;
    public GameObject no_tether_mode;
    public GameObject tether_on_mode;
    public GameObject chained_mode;
    public GameObject one_hit_kill_mode;

    public bool selected_level = false;

    void Awake()
    {
        level_manager = this;
    }
    void Start()
    {
        line = this.GetComponent<LineRenderer>();
        game_mode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<Mode>();

        foreach (Transform child in transform)
            levels.Add(child.gameObject);

        line.SetVertexCount((levels.Count * 2) - 1);
        line_2.SetVertexCount((levels.Count * 2) - 1);
        Vector3[] positions = new Vector3[levels.Count * 2 - 1];

        for (int x = 0; x < (levels.Count * 2) - 1; x++)
        {
            if (x % 2 == 0)     // Even number
            {
                positions[x] = new Vector3(levels[x / 2].transform.position.x, levels[x / 2].transform.position.y, 0.01f);
            }
            else if ((x / 2) + 1 < levels.Count)        // Odd number
            {
                positions[x] = new Vector3((levels[x / 2].transform.position.x + levels[(x / 2) + 1].transform.position.x) / 2,
                    (levels[x / 2].transform.position.y + levels[(x / 2) + 1].transform.position.y) / 2, 
                    0.01f);
            }
        }
        line.SetPositions(positions);
        Array.Reverse(positions);
        line_2.SetPositions(positions);
        /*
        for (int x = (levels.Count - 1); x >= 0; x--)
        {
            line_2.SetPosition(x, new Vector3(levels[x].transform.position.x, 
                levels[x].transform.position.y,
                0.01f));
        }*/


        level_nodes = this.GetComponentsInChildren<LevelNode>();
	}
	

    public void HoverOverLevel(LevelNode level)
    {
        selected_level_ui.SetActive(true);

        //selected_level_ui.transform.position = level.transform.position;
        RectTransform r = selected_level_ui.GetComponent<RectTransform>();
        //r.offsetMax = level.transform.position;
        r.anchoredPosition = level.transform.position;

        level_text_name.text = level.level_name;
        level_settings.SetActive(false);

        SoundMixer.sound_manager.Play8bitBeep();
    }
    public void SelectLevel(LevelNode level)
    {
        selected_level_ui.SetActive(true);
        //selected_level_ui.transform.position = level.transform.position;
        level_text_name.text = level.level_name;
        game_mode.SetLevelToLoad(level.level_to_load);
        level_settings.SetActive(true);

        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(object_to_select);

        // Play sound
        SoundMixer.sound_manager.PlayNotification();
        selected_level = true;
    }
    public void DeselectLevels()
    {
        selected_level_ui.SetActive(false);
        level_settings.SetActive(false);
        foreach (LevelNode node in level_nodes)
        {
            node.DeselectLevel();
        }
        selected_level = false;
    }
}
