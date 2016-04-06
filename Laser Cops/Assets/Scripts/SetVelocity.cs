using UnityEngine;
using System.Collections;

public class SetVelocity : MonoBehaviour
{
    public direction travelDirection = direction.left;
    public float speed = 1f;

    void Awake ()
    {
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
