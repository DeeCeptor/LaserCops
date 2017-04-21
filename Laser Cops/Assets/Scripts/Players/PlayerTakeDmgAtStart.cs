using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerTakeDmgAtStart : MonoBehaviour 
{
    public float dmg = 30f;

    float counter = 0.5f;

	void Start () 
	{
		
	}
	

	void Update () 
	{
        counter -= Time.deltaTime;
        if (counter <= 0f)
        {
            this.GetComponent<PlayerController>().TakeHit(dmg, false);
            Debug.Log("Dmg player by: " + dmg);
            Destroy(this);
        }
	}
}
