using UnityEngine;
using System.Collections;

public class SlowlyMove : MonoBehaviour
{
    public bool follow_gravity = true;
    public bool dont_multiply_by_gravity = false;
    public float speed = 1.0f;

	void Start ()
    {
	
	}
	
	void Update ()
    {
	    if (follow_gravity)
        {
            if (dont_multiply_by_gravity)
                this.transform.position = (Vector2) this.transform.position + speed * Physics2D.gravity * Time.deltaTime;
            else
                this.transform.position = (Vector2)this.transform.position + speed * (Physics2D.gravity.normalized) * Time.deltaTime;

        }
        else
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
        }
	}
}
