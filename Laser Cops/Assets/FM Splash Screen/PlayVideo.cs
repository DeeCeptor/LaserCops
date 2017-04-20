using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVideo : MonoBehaviour 
{

	void Start () 
	{
        ((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
    }
	
}
