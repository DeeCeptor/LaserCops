using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAttachTextToTether : MonoBehaviour 
{
    public GameObject Destroy_msg;
    public GameObject Save;


	void Start () 
	{

    }


	void Update () 
	{
        if (Tether.tether.middle_link == null)
            return;

        this.transform.position = Tether.tether.middle_link.transform.position;

        if (Tether.tether.cur_tether_mode == Tether.TetherMode.Destroy)
        {
            Destroy_msg.SetActive(true);
            Save.SetActive(false);
        }
        else if (Tether.tether.cur_tether_mode == Tether.TetherMode.Capture)
        {
            Destroy_msg.SetActive(false);
            Save.SetActive(true);
        }
        else
        {
            Destroy_msg.SetActive(false);
            Save.SetActive(false);
        }
    }
}
