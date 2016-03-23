using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
    public Vector3 rotation_speed;

    void Start ()
    {
	
	}



	void Update ()
    {
        this.transform.eulerAngles += rotation_speed * Time.deltaTime;
	}
}
