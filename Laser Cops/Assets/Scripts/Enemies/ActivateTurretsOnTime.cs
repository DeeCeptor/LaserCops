using UnityEngine;
using System.Collections;

public class ActivateTurretsOnTime : MonoBehaviour {
    public float timeUntilActivation = 10f;
    private float activationCounter = 0;

	// Use this for initialization
	void Start () {
        activationCounter = timeUntilActivation + Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	    if(activationCounter<Time.time)
        {
            RayLaserScript[] turrets = transform.GetComponentsInChildren<RayLaserScript>();
            for (int i = 0; i < turrets.Length; i++)
            {
                turrets[i].active = true;
            }
            ForwardShotScript[] moreTurrets = transform.GetComponentsInChildren<ForwardShotScript>();
            for (int i = 0; i < turrets.Length; i++)
            {
                moreTurrets[i].active = true;
            }
        }
	}
}
