using UnityEngine;
using System.Collections;

public class PlayerController : PlayerInput
{
    Rigidbody2D physics;

    void Awake ()
    {
        physics = this.GetComponent<Rigidbody2D>();
    }
	void Start ()
    {

    }

    void Update ()
    {
        UpdateInputs();

        physics.velocity = this.direction;
	}
}
