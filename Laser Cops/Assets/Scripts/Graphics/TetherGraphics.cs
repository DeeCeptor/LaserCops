using UnityEngine;
using System.Collections;

public class TetherGraphics : MonoBehaviour
{
    LineRenderer line;

    public Color standard_colour;
    public Color pulsating_colour;

    public float left_width = 0.1f;
    public float right_width = 0.1f;

    Color cur_left;
    Color cur_right;

    void Awake ()
    {
        line = this.GetComponent<LineRenderer>();
    }
	void Start ()
    {
        cur_left = standard_colour;
        cur_right = pulsating_colour;
        line.SetColors(cur_left, cur_right);
        line.SetWidth(left_width, right_width);
    }
	


	void Update ()
    {
	    // Pulsates between the 2 colours, end from end
        //line.SetColors()
	}
}
