using UnityEngine;
using System.Collections.Generic;

public class PlayerController : PlayerInput
{
    Rigidbody2D physics;
    float x_speed = 3.5f;
    float y_speed = 3.5f;

    Vector2 screen_margins = new Vector2(0.2f, 0.2f);


    public float Max_Health = 100f;
    public float Health;


    // Car
    public GameObject car_sprite;   // Sprite we'll be rotating using animation
    float default_rotation; // Rotation we return to if doing nothing
    float desired_rotation;
    float rotation_changing_speed = 0.2f;   // How quickly we lerp between rotations
    float max_turning_rotation = 20f;
    float sideways_turning_speed = 50.0f;

    // SPARKS (we reuse the same spark gameobjects when grinding)
    Dictionary<GameObject, ParticleSystem> in_use_grinding_sparks = new Dictionary<GameObject, ParticleSystem>();
    List<ParticleSystem> free_grinding_sparks = new List<ParticleSystem>();
    public GameObject grinding_sparks;

    public List<ParticleSystem> moving_forward_particles = new List<ParticleSystem>();  // Boosters and stuff turn on when moving forward
    public List<ParticleSystem> moving_backwards_particles = new List<ParticleSystem>();    // Brakes for slowing down

    void Awake ()
    {
        physics = this.GetComponent<Rigidbody2D>();
        default_rotation = transform.rotation.eulerAngles.z;
        desired_rotation = default_rotation;

        Health = Max_Health;
    }
	void Start ()
    {
        GameState.game_state.Players.Add(this);
        UIManager.ui_manager.UpdateHealth();
    }

    void Update ()
    {
        UpdateInputs();

        Vector2 new_speed = new Vector2(this.direction.x * x_speed, this.direction.y * y_speed);
        physics.velocity = new_speed;

        if (tether_switched)
            Tether.tether.SwitchTether();

        // Animate car turning
        // Animate breaking/accelerating forwards
        if (GameState.game_state.going_sideways)
        {
            TurningCar(new_speed.y);
            AccelerateDecelartingCar(new_speed.x);
        }
        else
        {
            Debug.Log("A");
            TurningCar(new_speed.x);
            AccelerateDecelartingCar(new_speed.y);
        }

        // Force the player to remain within view of the camera
        StayOnScreen();

        // ROTATION
        // Lerp to our desired rotation
        physics.MoveRotation(Mathf.Lerp(transform.eulerAngles.z, desired_rotation, rotation_changing_speed));
        //transform.eulerAngles = new Vector3(0, 0, 
        //    Mathf.Lerp(transform.eulerAngles.z, desired_rotation, 0.01f));
    }
    // Slightly rotate car to make it look like turning
    public void TurningCar(float amount)
    {
        if (amount > 0)
        {
            // Sideways: going left
            // Turn more as we keep turning
            desired_rotation = Mathf.Min(desired_rotation + (Time.deltaTime * sideways_turning_speed), default_rotation + max_turning_rotation);
            rotation_changing_speed = 0.2f;
        }
        else if (amount < 0)
        {
            // Sideways: going right
            desired_rotation = Mathf.Max(desired_rotation - (sideways_turning_speed * Time.deltaTime), default_rotation - max_turning_rotation);
            rotation_changing_speed = 0.2f;
        }
        else
        {
            // Not turning, return to normal rotation
            desired_rotation = default_rotation;
            rotation_changing_speed = 0.05f;
        }
    }
    public void AccelerateDecelartingCar(float amount)
    {
        // Accelerating
        if (amount > 0)
        {
            // Turn on accelerating
            foreach (ParticleSystem ps in moving_forward_particles)
            {
                if (!ps.isPlaying)
                    ps.Play();
            }
            // Turn off braking
            foreach (ParticleSystem ps in moving_backwards_particles)
            {
                if (ps.isPlaying)
                    ps.Stop();
            }
        }
        // Decelerating
        else if (amount < 0)
        {
            // Turn off acceleration
            foreach (ParticleSystem ps in moving_forward_particles)
            {
                if (ps.isPlaying)
                    ps.Stop();
            }
            // Turn on braking
            foreach (ParticleSystem ps in moving_backwards_particles)
            {
                if (!ps.isPlaying)
                    ps.Play();
            }
        }
        else
        {
            // Turn off acceleration
            foreach (ParticleSystem ps in moving_forward_particles)
            {
                if (ps.isPlaying)
                    ps.Stop();
            }
            // Turn off braking
            foreach (ParticleSystem ps in moving_backwards_particles)
            {
                if (ps.isPlaying)
                    ps.Stop();
            }
        }
    }


    public void TakeHit(float damage)
    {
        Health -= damage;
        Health = Mathf.Max(0, Health);

        // Set health bar

        if (Health <= 0)
            Die();
    }
    public void Die()
    {
        Debug.Log("Player " + player_number + " died");

        ClearGrindingSparks();

        // Destroy tether
        Tether.tether.DestroyTether();

        Destroy(gameObject);
    }
    public void ClearGrindingSparks()
    {
        // Remove grinding sparks
        List<ParticleSystem> sparks = new List<ParticleSystem>();
        foreach (KeyValuePair<GameObject, ParticleSystem> entry in in_use_grinding_sparks)
        {
            sparks.Add(entry.Value);
        }
        foreach (ParticleSystem ps in free_grinding_sparks)
        {
            sparks.Add(ps);
        }
        for (int x = 0; x < sparks.Count; x++)
        {
            Destroy(sparks[x].gameObject);
        }
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

    // Shower of sparks on a collision!
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Show sparks on the side of the car it was hit on
        CollisionAt(collision.contacts[0].point);

        ParticleSystem sparks;
        // New collision, grab a grinding sparks if we've used one before
        if (free_grinding_sparks.Count > 0)
        {
            sparks = free_grinding_sparks[0];
            free_grinding_sparks.RemoveAt(0);
            sparks.Play();
        }
        else
        {
            // Need to spawn a new grinding sparks
            sparks = ((GameObject) GameObject.Instantiate(grinding_sparks)).GetComponent<ParticleSystem>();
        }

        // Set its position and add it to the dictionary
        sparks.gameObject.transform.position = collision.contacts[0].point;

        if (!in_use_grinding_sparks.ContainsKey(collision.gameObject))
            in_use_grinding_sparks.Add(collision.gameObject, sparks);
    }
    // Show grinding sparks when touching another object
    void OnCollisionStay2D(Collision2D coll)
    {
        // Update the position of the grinding
        in_use_grinding_sparks[coll.gameObject].gameObject.transform.position = coll.contacts[0].point;
    }
    // Stop grinding against the object we were pushing against
    void OnCollisionExit2D(Collision2D coll)
    {
        ParticleSystem sparks = in_use_grinding_sparks[coll.gameObject];
        sparks.Stop();
        in_use_grinding_sparks.Remove(sparks.gameObject);
        free_grinding_sparks.Add(sparks);
    }

    public void CollisionAt(Vector2 position)
    {

    }
}
