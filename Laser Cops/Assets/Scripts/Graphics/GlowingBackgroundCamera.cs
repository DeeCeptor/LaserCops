using UnityEngine;
using System.Collections;

public class GlowingBackgroundCamera : MonoBehaviour 
{
    public static GlowingBackgroundCamera background_cam;

    void Awake () 
	{
        background_cam = this;
	}
}
