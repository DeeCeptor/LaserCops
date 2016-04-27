using UnityEngine;
using System.Collections;

public class PositionBoundaryOffscreen : MonoBehaviour
{
    BoxCollider2D box;
    GameObject highway;
    public enum Match_Screen_Or_Highway { Screen, Highway };
    public Match_Screen_Or_Highway Matching;
    public enum Side_Of_Screen { Left, Right, Top, Bottom };
    public Side_Of_Screen Side;

    void Start ()
    {
        box = this.GetComponent<BoxCollider2D>();
        highway = GameObject.FindGameObjectWithTag("Grid");

        switch (Matching)
        {
            case Match_Screen_Or_Highway.Screen:
                AdjustPositionByScreen();
                break;
            case Match_Screen_Or_Highway.Highway:
                AdjustPositionByHighway();
                break;
        }
    }


    public void AdjustPositionByScreen()
    {
        Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        Vector2 new_position = Vector2.zero;
        switch (Side)
        {
            case Side_Of_Screen.Left:
                new_position = new Vector2(minScreenBounds.x - box.bounds.extents.x, 0);
                break;
            case Side_Of_Screen.Right:
                new_position = new Vector2(maxScreenBounds.x + box.bounds.extents.x, 0);
                break;
            case Side_Of_Screen.Bottom:
                new_position = new Vector2(0, minScreenBounds.y - box.bounds.extents.y);
                break;
            case Side_Of_Screen.Top:
                new_position = new Vector2(0, maxScreenBounds.y + box.bounds.extents.y);
                break;
        }

        this.transform.position = new_position;
    }
    public void AdjustPositionByHighway()
    {
        MeshRenderer mesh = highway.GetComponent<MeshRenderer>();
        Vector3 minScreenBounds = mesh.bounds.min;
        Vector3 maxScreenBounds = mesh.bounds.max;

        Vector2 new_position = Vector2.zero;
        switch (Side)
        {
            case Side_Of_Screen.Left:
                new_position = new Vector2(minScreenBounds.x - box.bounds.extents.x, 0);
                break;
            case Side_Of_Screen.Right:
                new_position = new Vector2(maxScreenBounds.x + box.bounds.extents.x, 0);
                break;
            case Side_Of_Screen.Bottom:
                new_position = new Vector2(0, minScreenBounds.y - box.bounds.extents.y);
                break;
            case Side_Of_Screen.Top:
                new_position = new Vector2(0, maxScreenBounds.y + box.bounds.extents.y);
                break;
        }

        this.transform.position = new_position;
    }


    void Update () {
	
	}
}
