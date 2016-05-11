using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public float desired_size = 7.0f;   // How large the camera is zooming to
    float zoom_speed = 0.5f;        // How quickly the zoom changes
    Camera cam;

    BoxCollider2D camera_region;

    void Awake ()
    {
        cam = Camera.main;
        ChangeZoom(this.cam.orthographicSize, 0.5f);
        camera_region = this.GetComponent<BoxCollider2D>();
    }
    void Start ()
    {
        Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        camera_region.size = new Vector2((Mathf.Abs(minScreenBounds.x) + Mathf.Abs(maxScreenBounds.x)) / 1 * this.transform.localScale.x,
                                         (Mathf.Abs(minScreenBounds.y) + Mathf.Abs(maxScreenBounds.y)) / 1 * this.transform.localScale.y);

    }


    public void ChangeZoom(float zoom_level, float speed)
    {
        desired_size = zoom_level;
        zoom_speed = speed;
    }


    void Update ()
    {
	    if (this.cam.orthographicSize != desired_size)
        {
            if (this.cam.orthographicSize < desired_size)
            {
                this.cam.orthographicSize = Mathf.Min(desired_size, this.cam.orthographicSize + Time.deltaTime * zoom_speed);
            }
            else
            {
                this.cam.orthographicSize = Mathf.Max(desired_size, this.cam.orthographicSize - Time.deltaTime * zoom_speed);
            }
        }
	}
}
