using UnityEngine;
using System.Collections;

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
        set { show_wakes = value; }
    }
    private bool show_wakes = true;

    void Awake()
    {
        graphical_settings = this;
    }
    void Start () 
	{
	
	}
	

	void Update () 
	{
	
	}
}
