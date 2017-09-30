using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveIfBossDies : MonoBehaviour 
{
    public GameObject current_boss;

	void Start () 
	{
        current_boss = GameObject.FindGameObjectWithTag("Boss");

        if (current_boss == null)
        {
            Debug.LogError(this.gameObject.name + " can't find current boss");
            Destroy(this);
        }
	}
	

	void Update () 
	{
		if (current_boss == null)
        {
            Debug.Log("Boss died. Removing " + this.gameObject.name);
            Destroy(this.gameObject);
        }
	}
}
