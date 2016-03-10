using UnityEngine;
using System.Collections.Generic;

public class Tether : MonoBehaviour
{
    public static Tether tether;

    LineRenderer line;

    public Color standard_colour;
    public Color pulsating_colour;

    public float left_width = 0.1f;
    public float right_width = 0.1f;

    Color cur_left;
    Color cur_right;

    float cur_tether_switching_cooldown;
    float tether_switching_cooldown = 0.4f;

    public enum TetherMode { None, Destroy, Capture };
    public TetherMode cur_tether_mode = TetherMode.Destroy;

    public List<GameObject> tether_links;

    void Awake ()
    {
        tether = this;
        line = this.GetComponent<LineRenderer>();
    }
	void Start ()
    {
        cur_left = standard_colour;
        cur_right = pulsating_colour;
        line.SetColors(cur_left, cur_right);
        line.SetWidth(left_width, right_width);

        SetTetherMode(TetherMode.Destroy);
    }


    public void SwitchTether()
    {
        if (cur_tether_switching_cooldown <= 0)
        {
            cur_tether_switching_cooldown = tether_switching_cooldown;

            if (cur_tether_mode == TetherMode.Destroy)
                SetTetherMode(TetherMode.Capture);
            else if (cur_tether_mode == TetherMode.Capture)
                SetTetherMode(TetherMode.Destroy);
        }
    }
    public void SetTetherMode(TetherMode mode)
    {
        if (mode == TetherMode.Destroy)
        {

        }
        else if (mode == TetherMode.Capture)
        {

        }
        Debug.Log("Setting tether " + mode);
        cur_tether_mode = mode;
    }


	void Update ()
    {
        cur_tether_switching_cooldown -= Time.deltaTime;
	    // Pulsates between the 2 colours, end from end
        //line.SetColors()
	}
}
