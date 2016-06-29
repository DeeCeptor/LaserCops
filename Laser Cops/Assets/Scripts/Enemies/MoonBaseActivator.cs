using UnityEngine;
using System.Collections;

public class MoonBaseActivator : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("hi");
        GameObject.Find("MoonBase").GetComponent<SecretMoonBossLaser>().Activate();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
