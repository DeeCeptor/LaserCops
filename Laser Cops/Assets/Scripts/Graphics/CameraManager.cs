using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public float desired_size = 7.0f;   // How large the camera is zooming to
    float zoom_speed = 0.5f;        // How quickly the zoom changes
    Camera cam;

    void Awake ()
    {
        cam = Camera.main;
        ChangeZoom(desired_size, 0.5f);
    }
    void Start ()
    {
	
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
