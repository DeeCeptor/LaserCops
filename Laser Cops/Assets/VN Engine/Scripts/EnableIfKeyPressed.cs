using UnityEngine;
using System.Collections;

// Listens for any buttons presses, and sets the given object to active and disables this object
public class EnableIfKeyPressed : MonoBehaviour
{
    public GameObject object_to_enable;
	
	void Update ()
    {
	    if (Input.anyKeyDown)
        {
            object_to_enable.SetActive(true);
            this.gameObject.SetActive(false);
        }
	}
}
