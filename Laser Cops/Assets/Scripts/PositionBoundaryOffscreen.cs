using UnityEngine;
using System.Collections;

public class PositionBoundaryOffscreen : MonoBehaviour
{
    BoxCollider2D box;
    public bool left_side_of_screen = true;

    void Start ()
    {
        box = this.GetComponent<BoxCollider2D>();
        AdjustPosition();
    }


    public void AdjustPosition()
    {
        Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        Vector2 new_position;
        if (left_side_of_screen)
        {
            new_position = new Vector2(minScreenBounds.x - box.bounds.extents.x, 0);
        }
        else
        {
            // Right side
            new_position = new Vector2(maxScreenBounds.x + box.bounds.extents.x, 0);
        }

        this.transform.position = new_position;
    }


    void Update () {
	
	}
}
