using UnityEngine;
using System.Collections;

// Resizes logo based on resolution
public class ResizeLogo : MonoBehaviour
{
    public GameObject logo;

	void Awake ()
    {
        Vector2 cur_aspect = AspectRatio.GetAspectRatio(Screen.width, Screen.height);
        Debug.Log("Running at " + cur_aspect);
        if (cur_aspect.x == 16.0f
            && (cur_aspect.y == 9.0f || cur_aspect.y == 10.0f))   // Check for 16:9 or 16:10 aspect. 
        {
            
        }
        else
            logo.transform.localScale = new Vector3(0.8f, 0.8f, 1); // If not, downsize logo so it fits on-screen
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}


public static class AspectRatio
{
    public static Vector2 GetAspectRatio(int x, int y)
    {
        float f = (float)x / (float)y;
        int i = 0;
        while (true)
        {
            i++;
            if (System.Math.Round(f * i, 2) == Mathf.RoundToInt(f * i))
                break;
        }
        return new Vector2((float)System.Math.Round(f * i, 2), i);
    }
    public static Vector2 GetAspectRatio(Vector2 xy)
    {
        float f = xy.x / xy.y;
        int i = 0;
        while (true)
        {
            i++;
            if (System.Math.Round(f * i, 2) == Mathf.RoundToInt(f * i))
                break;
        }
        return new Vector2((float)System.Math.Round(f * i, 2), i);
    }
    public static Vector2 GetAspectRatio(int x, int y, bool debug)
    {
        float f = (float)x / (float)y;
        int i = 0;
        while (true)
        {
            i++;
            if (System.Math.Round(f * i, 2) == Mathf.RoundToInt(f * i))
                break;
        }
        if (debug)
            Debug.Log("Aspect ratio is " + f * i + ":" + i + " (Resolution: " + x + "x" + y + ")");
        return new Vector2((float)System.Math.Round(f * i, 2), i);
    }
    public static Vector2 GetAspectRatio(Vector2 xy, bool debug)
    {
        float f = xy.x / xy.y;
        int i = 0;
        while (true)
        {
            i++;
            if (System.Math.Round(f * i, 2) == Mathf.RoundToInt(f * i))
                break;
        }
        if (debug)
            Debug.Log("Aspect ratio is " + f * i + ":" + i + " (Resolution: " + xy.x + "x" + xy.y + ")");
        return new Vector2((float)System.Math.Round(f * i, 2), i);
    }
}