using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GraphicalSettings : MonoBehaviour 
{
    public static GraphicalSettings graphical_settings;

    public bool Show_Planets
    {
        get { return show_planets; }
        set { show_planets = value; }
    }
    private bool show_planets = true;
    public bool Show_Skybox // Shows the skybox
    {
        get { return show_skybox; }
        set { show_skybox = value; }
    }
    private bool show_skybox = true;
    public bool Scroll_Grid  // Whether the grid moves or not
    {
        get { return scroll_grid; }
        set { scroll_grid = value; }
    }
    private bool scroll_grid = true;
    public bool Show_Wakes  // Whether cars create wakes with the highway grid
    {
        get { return show_wakes; }
        set {
            show_wakes = value;
            SaveGraphicalSetting("Show_Wakes", value);
        }
    }
    private bool show_wakes = true;

    public Toggle Show_Wakes_Toggle;


    void Awake()
    {
        graphical_settings = this;
    }
    void Start () 
	{
        LoadGraphicalSettings();
	}
	
    public void LoadGraphicalSettings()
    {
        Show_Wakes = System.Convert.ToBoolean(PlayerPrefs.GetInt("Show_Wakes", 1));
        Show_Wakes_Toggle.isOn = Show_Wakes;
    }
    public void SaveGraphicalSetting(string key, bool value)
    {
        PlayerPrefs.SetInt(key, System.Convert.ToInt32(value));
        PlayerPrefs.Save();
    }

    void Update () 
	{
	
	}
}
