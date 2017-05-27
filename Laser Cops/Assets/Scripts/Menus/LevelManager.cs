using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour 
{
    public static LevelManager level_manager;
    //List<GameObject> levels = new List<GameObject>();
    //LineRenderer line;
    //public LineRenderer line_2;
    [HideInInspector]
    public LevelNode[] level_nodes;
    //Mode game_mode;
    public Text high_score_text;
    public Text hint_text;

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

        level_nodes = Transform.FindObjectsOfType<LevelNode>();
    }
    void Start()
    {

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
        Mode.current_mode.SetLevelToLoad(level.level_to_load);
        level_settings.SetActive(true);

        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(object_to_select);

        // Play sound
        //SoundMixer.sound_manager.PlayNotification();
        selected_level = true;

        // Set to coop game mode
        //coop_mode.GetComponent<Toggle>().Select();
        var pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(coop_mode.gameObject, pointer, ExecuteEvents.pointerClickHandler);
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



    public void LoadSelectedLevel()
    {
        try
        {
            Mode.current_mode.Load_Level();
        }
        catch (Exception e)
        {
            Debug.LogError("Could not find current game mode to load level with " + e.Message);
        }
    }
}
