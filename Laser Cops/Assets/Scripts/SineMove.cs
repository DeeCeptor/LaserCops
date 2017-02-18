using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineMove : MonoBehaviour
{
    Vector3 start_pos;

    void Awake ()
    {
        start_pos = this.transform.position;
    }


    void Update ()
    {
        this.transform.position = start_pos + new Vector3(Mathf.Sin(Time.time * 0.3f) * 10f, Mathf.Sin(Time.time * 0.1f) * 5f, 0);
    }
}
