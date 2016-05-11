using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class LevelManager : MonoBehaviour 
{
    public static LevelManager level_manager;
    List<GameObject> levels = new List<GameObject>();
    LineRenderer line;
    LevelNode[] level_nodes;
    Mode game_mode;

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

        line.SetVertexCount(levels.Count);
        for (int x = 0; x < levels.Count; x++)
        {
            line.SetPosition(x, new Vector3(levels[x].transform.position.x, levels[x].transform.position.y, 0.01f));
        }

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
    }
    public void DeselectLevels()
    {
        selected_level_ui.SetActive(false);
        level_settings.SetActive(false);
        foreach (LevelNode node in level_nodes)
        {
            node.DeselectLevel();
        }
    }
    

	void Update () 
	{
	
	}
}
