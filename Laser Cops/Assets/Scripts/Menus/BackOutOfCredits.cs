using UnityEngine;
using System.Collections;

public class BackOutOfCredits : MonoBehaviour 
{
    public GameObject credits;
    public GameObject menu;
    public GameObject logo;

	void Start () 
	{
	
	}
	

	void Update () 
	{
	    if (Input.anyKeyDown)
        {
            menu.SetActive(true);
            credits.SetActive(false);
            logo.SetActive(true);
        }
	}
}
