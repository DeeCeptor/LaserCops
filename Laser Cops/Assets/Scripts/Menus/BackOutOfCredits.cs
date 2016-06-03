using UnityEngine;
using System.Collections;

public class BackOutOfCredits : MonoBehaviour 
{
    public GameObject credits;
    public GameObject menu;

	void Start () 
	{
	
	}
	

	void Update () 
	{
	    if (Input.anyKeyDown)
        {
            menu.SetActive(true);
            credits.SetActive(false);
        }
	}
}
