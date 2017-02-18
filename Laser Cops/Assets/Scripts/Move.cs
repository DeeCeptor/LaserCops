using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Vector3 move_in_this_direction;

    public bool move_back_to_starting_pos_on_awake = false;
    Vector3 starting_pos;

    void Awake ()
    {
        Debug.Log("awake");
        starting_pos = this.transform.position;
	}


    void OnEnable()
    {
        Debug.Log("OnEnable");
        this.transform.position = starting_pos;
    }


    void Update ()
    {
        this.transform.position += move_in_this_direction * Time.deltaTime;
    }
}
