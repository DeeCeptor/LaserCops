using UnityEngine;
using System.Collections.Generic;

public class PlayerController : PlayerInput
{
    Rigidbody2D physics;
    float x_speed = 6f;
    float y_speed = 6f;

    // OBSTACLES
    bool touching_forward_obstacle = false;
    float cur_touching_forward_obstacle = 0f;
    float touching_forward_obstacle_cooldown = 0.5f;

    Vector2 screen_margins = new Vector2(0.2f, 0.2f);

    // Grid attributes
    public Color primary_colour;
    float normal_grid_force = 1f;
    float normal_grid_radius = 1f;
    float boost_grid_force = 3f;
    float boost_grid_radius = 2f;

    public float Max_Health = 100f;
    public float Health;

    public float Grinding_Damage = 0.3f;    // How much damage we do by grinding against enemies

    // Boost
    float boost_cooldown = 1.5f;
    float boost_cur_cooldown = 0f;
    float boost_duration = .5f;
    float boost_cur_duration = 0f;
    float boost_speed_modifier = 2f;

    // Car
    public GameObject car_sprite;   // Sprite we'll be rotating using animation
    public float default_rotation; // Rotation we return to if doing nothing
    public float desired_rotation;
    float rotation_changing_speed = 0.2f;   // How quickly we lerp between rotations
    float max_turning_rotation = 20f;
    float sideways_turning_speed = 50.0f;

    // SPARKS (we reuse the same spark gameobjects when grinding)
    Dictionary<GameObject, ParticleSystem> in_use_grinding_sparks = new Dictionary<GameObject, ParticleSystem>();
    List<ParticleSystem> free_grinding_sparks = new List<ParticleSystem>();
    public GameObject grinding_sparks;


    // Particles
    public List<ParticleSystem> moving_forward_particles = new List<ParticleSystem>();  // Boosters and stuff turn on when moving forward
    public List<ParticleSystem> moving_backwards_particles = new List<ParticleSystem>();    // Brakes for slowing down

    public List<ParticleSystem> booster_particles = new List<ParticleSystem>();     // Happens when you boost

    public List<ParticleSystem> minor_damage_particles = new List<ParticleSystem>();
    public List<ParticleSystem> major_damage_particles = new List<ParticleSystem>();

    
    void Awake()
    {
        physics = this.GetComponent<Rigidbody2D>();
        default_rotation = transform.rotation.eulerAngles.z;
        desired_rotation = default_rotation;

        Health = Max_Health;
    }
    void Start()
    {
        GameState.game_state.Players.Add(this);
        UIManager.ui_manager.UpdateHealth();
    }

    void Update()
    {
        UpdateInputs();

        Vector2 new_speed = new Vector2(this.direction.x * x_speed, this.direction.y * y_speed);
        float grid_ripple_force = normal_grid_force;
        float grid_ripple_radius = normal_grid_radius;

        // BOOST
        boost_cur_cooldown -= Time.deltaTime;
        boost_cur_duration -= Time.deltaTime;
        // Check if we boosted
        if (boosted_this_instant)
        {
            // Is the boost on cooldown?
            if (boost_cur_cooldown <= 0)
            {
                // WE BOOSTIN
                foreach (ParticleSystem p in booster_particles)
                {
                    p.Play();
                }

                boost_cur_cooldown = boost_cooldown;
                boost_cur_duration = boost_duration;
            }
        }

        if (boost_cur_duration > 0)
        {
            // Apply boost speed!
            if (GameState.game_state.going_sideways)
            {
                new_speed.x = x_speed * boost_speed_modifier;
                new_speed.y = new_speed.y * boost_speed_modifier;
            }
            else
            {
                new_speed.y = y_speed * boost_speed_modifier;
                new_speed.x = new_speed.x * boost_speed_modifier;
            }
            grid_ripple_force = boost_grid_force;
            grid_ripple_radius = boost_grid_radius;
            SoundMixer.sound_manager.PlayCarRev();
        }


        // Can only steer properly when we're not caught on an obstacle
        if (GameState.game_state.tether_touching_obstacle)
        {
            if (GameState.game_state.going_sideways)
            {
                physics.velocity = new Vector2(physics.velocity.x, new_speed.y);
            }
            else
            {
                physics.velocity = new Vector2(new_speed.x, physics.velocity.y);
            }
        }
        else
        {
            // Set our actual velocity
            physics.velocity = new_speed;
        }

        if (disable_tether_held_down)
        {
            Tether.tether.TetherHeldDown();
        }
        else
            Tether.tether.TetherReleased();

        if (tether_switched)
            Tether.tether.SwitchTether();

        // Animate car turning
        // Animate breaking/accelerating forwards
        if (GameState.game_state.going_sideways)
        {
            TurningCar(new_speed.y);
            AccelerateDeceleratingCar(new_speed.x);
        }
        else
        {
            TurningCar(new_speed.x);
            AccelerateDeceleratingCar(new_speed.y);
        }

        // OBSTACLES
        cur_touching_forward_obstacle -= Time.deltaTime;
        if (cur_touching_forward_obstacle <= 0)
        {
            touching_forward_obstacle = false;
        }

        // Force the player to remain within view of the camera
        //StayOnScreen();

        // ROTATION
        // Lerp to our desired rotation
        physics.MoveRotation(Mathf.Lerp(transform.eulerAngles.z, desired_rotation, rotation_changing_speed));
        //transform.eulerAngles = new Vector3(0, 0, 
        //    Mathf.Lerp(transform.eulerAngles.z, desired_rotation, 0.01f));


        // Ripple the grid behind the car
        EffectsManager.effects.GridExplosion((Vector2)transform.position, grid_ripple_force, grid_ripple_radius, primary_colour);
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
            rotation_changing_speed = 0.1f;
        }
    }
    public void AccelerateDeceleratingCar(float amount)
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
        if (GameState.game_state.debug_invulnerability)
            damage = 0;

        float prev_health = Health;

        Health -= damage;
        Health = Mathf.Max(0, Health);

        // Set health bar
        UIManager.ui_manager.UpdateHealth();

        // Turn on smoke and fire to show the player is damaged
        if (prev_health > 50f && Health < 50f)
        {
            foreach (ParticleSystem p in minor_damage_particles)
            {
                if (!p.isPlaying)
                    p.Play();
            }
        }
        else if (prev_health > 25f && Health < 25f)
        {
            foreach (ParticleSystem p in major_damage_particles)
            {
                if (!p.isPlaying)
                    p.Play();
            }
        }

        if (Health <= 0)
            Die();
    }
    public void Die()
    {
        Debug.Log("Player " + player_number + " died");

        SoundMixer.sound_manager.Play8bitExplosion();
        ClearGrindingSparks();

        // Destroy tether
        if (Tether.tether != null)
            Tether.tether.DestroyTether();

        EffectsManager.effects.ViolentExplosion(this.transform.position);
        EffectsManager.effects.GridExplosion(this.transform.position, 1f, 15f, primary_colour);

        GameState.game_state.ChangeTimescale(0.3f);

        this.gameObject.layer = LayerMask.NameToLayer("Dead Player");
        this.gameObject.tag = "Obstacle";
        this.gameObject.AddComponent<PlayerDying>();

        GameState.game_state.Players.Remove(this);
        GameState.game_state.GameOver();

        Destroy(this);
        //Destroy(gameObject);
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
            if (sparks != null && sparks[x] != null && sparks[x].gameObject != null)
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            return;



        // SPARKS
        // Show sparks on the side of the car it was hit on
        CollisionAt(collision.contacts[0].point);

        ParticleSystem sparks;
        // New collision, grab a grinding sparks if we've used one before
        if (free_grinding_sparks.Count > 0)
        {
            sparks = free_grinding_sparks[0];
            free_grinding_sparks.RemoveAt(0);
            sparks.GetComponent<TurnOffSparks>().StartSparks();
        }
        else
        {
            // Need to spawn a new grinding sparks
            sparks = ((GameObject)GameObject.Instantiate(grinding_sparks)).GetComponent<ParticleSystem>();
        }

        // Set its position and add it to the dictionary
        sparks.gameObject.transform.position = collision.contacts[0].point;

        if (!in_use_grinding_sparks.ContainsKey(collision.gameObject))
            in_use_grinding_sparks.Add(collision.gameObject, sparks);

        //sparks.GetComponent<TurnOffSparks>().time_remaining = sparks.GetComponent<TurnOffSparks>().start_time_remaining;
    }
    // Show grinding sparks when touching another object
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject == null)
            return;



        // SPARKS
        // Update the position of the grinding
        if (in_use_grinding_sparks.ContainsKey(coll.gameObject) && in_use_grinding_sparks[coll.gameObject] != null)
        {
            ParticleSystem p = in_use_grinding_sparks[coll.gameObject];
            p.gameObject.transform.position = coll.contacts[0].point;
            p.GetComponent<TurnOffSparks>().StartSparks();
        }
        else
            in_use_grinding_sparks.Remove(coll.gameObject);
    }
    // Stop grinding against the object we were pushing against
    void OnCollisionExit2D(Collision2D coll)
    {
        ParticleSystem sparks = in_use_grinding_sparks[coll.gameObject];
        sparks.GetComponent<TurnOffSparks>().StopSparks();
        in_use_grinding_sparks.Remove(sparks.gameObject);
        free_grinding_sparks.Add(sparks);
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        // Check to see if we die
        if (coll.gameObject.layer == LayerMask.NameToLayer("Death Zone") 
            && (GameState.game_state.tether_touching_obstacle || touching_forward_obstacle))
        {
            HitDeathZone();
            return;
        }
        else
        {
            if (coll.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
            {
                cur_touching_forward_obstacle = touching_forward_obstacle_cooldown;
                touching_forward_obstacle = true;
            }
        }
    }
    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            cur_touching_forward_obstacle = touching_forward_obstacle_cooldown;
            touching_forward_obstacle = true;
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            cur_touching_forward_obstacle = 0;
            touching_forward_obstacle = false;
        }
    }

    public void CollisionAt(Vector2 position)
    {

    }

    public void HitDeathZone()
    {
        // Have other player die too
        GameState.game_state.PlayerHitDeathzone();
    }


    void OnDestroy()
    {
        ClearGrindingSparks();
    }
}
