using UnityEngine;
using System.Collections;

//simple script for object to scroll with no additional features
public class ScrollScript : MonoBehaviour {

	public float speed = 1f;
	public direction travelDirection = direction.left;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (travelDirection == direction.left)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, 0);
		}
		else if (travelDirection == direction.up)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
		}
		else if (travelDirection == direction.right)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
		}
		else if (travelDirection == direction.down)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, -speed);
		}
	}
}
