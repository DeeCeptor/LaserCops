using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAttachTextToTether : MonoBehaviour 
{
    public GameObject Destroy;
    public GameObject Save;


	void Start () 
	{
        StartCoroutine(AttachHalfwayThroughTether());
	}
	
    IEnumerator AttachHalfwayThroughTether()
    {
        yield return 0;
        yield return new WaitForSeconds(0.5f);

    }


	void Update () 
	{
        this.transform.position = Tether.tether.middle_link.transform.position;

        if (Tether.tether.cur_tether_mode == Tether.TetherMode.Destroy)
        {
            Destroy.SetActive(true);
            Save.SetActive(false);
        }
        else if (Tether.tether.cur_tether_mode == Tether.TetherMode.Capture)
        {
            Destroy.SetActive(false);
            Save.SetActive(true);
        }
        else
        {
            Destroy.SetActive(false);
            Save.SetActive(false);
        }
    }
}
