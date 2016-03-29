using UnityEngine;
using System.Collections;

public class RotateTowardsMoveDirection : MonoBehaviour
{
    Rigidbody2D physics;


    void Start ()
    {
        physics = this.GetComponent<Rigidbody2D>();
	}


    void Update ()
    {
        
    }
}
