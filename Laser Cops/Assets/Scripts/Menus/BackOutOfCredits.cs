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
	

    public void Turn_Off_Credits()
    {
        menu.SetActive(true);
        credits.SetActive(false);
        logo.SetActive(true);
    }


	void Update () 
	{
	    if (Input.anyKeyDown)
        {
            Turn_Off_Credits();
        }
	}
}
