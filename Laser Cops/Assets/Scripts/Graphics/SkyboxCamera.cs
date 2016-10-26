using UnityEngine;
using System.Collections;

public class SkyboxCamera : MonoBehaviour 
{
    public static SkyboxCamera skybox_camera;

	void Awake () 
	{
        skybox_camera = this;
	}
}
