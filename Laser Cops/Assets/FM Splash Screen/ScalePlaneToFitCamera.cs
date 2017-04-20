using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePlaneToFitCamera : MonoBehaviour 
{

	void Start () 
	{
        float height = 2.0f * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * 1f;
        float width = height * Camera.main.aspect;

        this.transform.localScale = new Vector3(width, height, 1);
    }
}
