using UnityEngine;
using System.Collections;

public class SlowlyMove : MonoBehaviour
{
    public bool follow_gravity = true;
    public float speed = 1.0f;

	void Start ()
    {
	
	}
	
	void Update ()
    {
	    if (follow_gravity)
        {
            this.transform.position = (Vector2) this.transform.position + speed * Physics2D.gravity * Time.deltaTime;
        }
        else
        {

        }
	}
}
