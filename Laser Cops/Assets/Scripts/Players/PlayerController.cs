using UnityEngine;
using System.Collections;

public class PlayerController : PlayerInput
{
    Rigidbody2D physics;
    float x_speed = 3.5f;
    float y_speed = 3.5f;

    Vector2 screen_margins = new Vector2(0.2f, 0.2f);

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

        physics.velocity = new Vector2(this.direction.x * x_speed, this.direction.y * y_speed);

        if (tether_switched)
            Tether.tether.SwitchTether();

        // Force the player to remain within view of the camera
        StayOnScreen();
    }

    void StayOnScreen()
    {
        /*
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 end_pos = this.transform.position;  // Where we will end up at the end of the frame

        if (screenPos.x < Screen.width)
            end_pos.x = screenPos.x;
        else if (screenPos.x > Screen.width)
            end_pos.x = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.width, 0)).x;

        // Adjust our actual position
        if (end_pos != this.transform.position)
            this.transform.position = end_pos;
        */
        Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minScreenBounds.x + screen_margins.x, maxScreenBounds.x - screen_margins.y), 
            Mathf.Clamp(transform.position.y, minScreenBounds.y + screen_margins.x, maxScreenBounds.y - screen_margins.y), 
            transform.position.z);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
