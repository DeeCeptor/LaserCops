using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using InControl;

public class PlayerController : PlayerInput
{
    public _Colour player_colour;

    public bool input_enabled = true;   // If false, no player input is accepted
    public bool alive = true;

    Rigidbody2D physics;
    float x_speed = 7f;
    float y_speed = 7f;

    // OBSTACLES
    bool touching_forward_obstacle = false;
    float cur_touching_forward_obstacle = 0f;
    float touching_forward_obstacle_cooldown = 0.5f;

    Vector2 screen_margins = new Vector2(0.2f, 0.2f);

    // Grid attributes
    public Color primary_colour;
    public float normal_grid_force = 1f;
    public float normal_grid_radius = 1.5f;
    public float boost_grid_force = 3f;
    public float boost_grid_radius = 2f;

    float wake_cooldown = 0.01f;
    float wake_counter = 0;

    [HideInInspector]
    public float grid_ripple_force = 0;
    public float grid_ripple_radius = 0;

    public float Max_Health = 100f;
    public float Health;
    float HP_transfer_rate = 15f;
    public float Grinding_Damage = 1f;    // How much damage we do by grinding against enemies
    public Image health_bar_image;

    // Boost
    float boost_cooldown = 1.5f;
    float boost_cur_cooldown = 0f;
    float boost_duration = .4f;
    float boost_cur_duration = 0f;
    float boost_speed_modifier = 3f;
    [HideInInspector]
    public bool currently_boosting = false;
    public float boosting_damage = 4.0f;

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

    // UI effects
    // UI dmg sparks
    public ParticleSystem UI_dmg_sparks;
    [HideInInspector]
    public float cur_spark_dmg_time;
    [HideInInspector]
    public float spark_dmg_time = 0.5f;
    // UI healing sparks
    public ParticleSystem UI_healing_sparks;
    [HideInInspector]
    public float cur_spark_healing_time;
    [HideInInspector]
    public float spark_healing_time = 1.5f;
    // UI healing transfer sparks
    public ParticleSystem UI_transfer_sparks;
    [HideInInspector]
    public float cur_spark_transfer_time;
    [HideInInspector]
    public float spark_transfer_time = 1.0f;
    // Low HP warning
    [HideInInspector]
    public GameObject UI_low_hp_warning;
    public ExpandToOriginalScale low_hp_red_circle;

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
    }
    void Start()
    {
        Max_Health = Max_Health * GameState.game_state.Player_Health_Modifier;  // Modify HP based on difficulty

        if (GameState.game_state.game_mode == GameState.GameMode.OneHitKill)
            this.Max_Health = 0.1f;

        Health = Max_Health;
        GameState.game_state.Players.Add(this);
        InGameUIManager.ui_manager.UpdateHealth();
        health_bar_image.color = new Color(health_bar_image.color.r, health_bar_image.color.g, health_bar_image.color.b, 0f);

        GameObject hp = GameObject.FindGameObjectWithTag(player_number + "HP");
        this.UI_dmg_sparks = hp.transform.Find("HP Effects/DmgSparks").GetComponent<ParticleSystem>();
        this.UI_healing_sparks = hp.transform.FindChild("HP Effects/HealingSparks").GetComponent<ParticleSystem>();
        this.UI_transfer_sparks = hp.transform.FindChild("Transfer Effects/HealingTransferSparks").GetComponent<ParticleSystem>();
        this.UI_low_hp_warning = hp.transform.FindChild("Low HP Warning").gameObject;


        // Get player inputs
        if (ControlsManager.Player_Controls.ContainsKey(player_number))
        {
            controller = ControlsManager.Player_Controls[player_number].device;

            if (controller != null)
                left_side_of_controller = ControlsManager.Player_Controls[player_number].left_or_right_side;
            else
            {
                // Controller is null, must be keyboard
                inputs_to_check = new List<string>();

                switch (player_number)
                {
                    case 1:
                        inputs_to_check.Add("Keyboard Left");
                        break;
                    case 2:
                        inputs_to_check.Add("Keyboard Right");
                        break;
                }
            }
        }
        else
        {
            // Couldn't find any inputs, just make some default ones
            inputs_to_check = new List<string>();
            Debug.Log("No inputs found");
            switch (player_number)
            {
                case 1:
                    inputs_to_check.Add("Keyboard Left");

                    if (InputManager.Devices != null && InputManager.Devices.Count >= 2)
                    {
                        controller = InputManager.Devices[0];
                        left_side_of_controller = true;
                    }
                    else if (InputManager.Devices != null && InputManager.Devices.Count >= 1 && GameState.game_state.number_of_players < 3)
                    {
                        controller = InputManager.Devices[0];
                        left_side_of_controller = true;
                    }
                    break;
                case 2:
                    inputs_to_check.Add("Keyboard Right");

                    if (InputManager.Devices != null && InputManager.Devices.Count >= 2)
                    {
                        controller = InputManager.Devices[0];
                        left_side_of_controller = false;
                    }
                    else if (InputManager.Devices != null && InputManager.Devices.Count >= 1 && GameState.game_state.number_of_players < 3)
                    {
                        controller = InputManager.Devices[0];
                        left_side_of_controller = false;
                    }
                    break;
                case 3:
                    inputs_to_check.Add("Keyboard Left");

                    if (InputManager.Devices != null && InputManager.Devices.Count >= 2)
                    {
                        controller = InputManager.Devices[1];
                        left_side_of_controller = true;
                    }
                    else if (InputManager.Devices != null && InputManager.Devices.Count >= 1)
                    {
                        controller = InputManager.Devices[0];
                        left_side_of_controller = true;
                    }
                    break;
                case 4:
                    inputs_to_check.Add("Keyboard Right");

                    if (InputManager.Devices != null && InputManager.Devices.Count >= 2)
                    {
                        controller = InputManager.Devices[1];
                        left_side_of_controller = false;
                    }
                    else if (InputManager.Devices != null && InputManager.Devices.Count >= 1)
                    {
                        controller = InputManager.Devices[0];
                        left_side_of_controller = false;
                    }
                    break;
            }
        }
    }


    void Update()
    {
        if (!GameState.game_state.paused)
        {
            if (input_enabled)
                UpdateInputs();
            else
                Disabled_Input();

            Vector2 new_speed = new Vector2(this.direction.x * x_speed, this.direction.y * y_speed);
            grid_ripple_force = normal_grid_force;
            grid_ripple_radius = normal_grid_radius;

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
                currently_boosting = true;
            }
            else
                currently_boosting = false;


            // Can only steer properly when we're not caught on an obstacle
            if (GameState.game_state.limit_player_control_from_obstacles)
            {
                if (GameState.game_state.going_sideways)
                {
                    //Debug.Log("A");
                    physics.velocity = new Vector2(physics.velocity.x, new_speed.y);
                    //physics.AddForce(new Vector2(new_speed.x * 0.25f, new_speed.y * 0.5f), ForceMode2D.Force);
                }
                else
                {
                    //physics.velocity = new Vector2(new_speed.x, physics.velocity.y);
                }
            }
            else
            {
                // Set our actual velocity
                physics.velocity = new_speed;
            }
            if (Tether.tether != null)
            {
                if (disable_tether_held_down && GameState.game_state.can_disable_tether)
                {
                    Tether.tether.TetherHeldDown();
                }
                else
                    Tether.tether.TetherReleased();

                if (tether_switched && GameState.game_state.can_change_tether_mode)
                    Tether.tether.SwitchTether();
            }
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


            //transform.eulerAngles = new Vector3(0, 0, 
            //    Mathf.Lerp(transform.eulerAngles.z, desired_rotation, 0.01f));

            // Start fading health bar
            health_bar_image.color = new Color(health_bar_image.color.r, health_bar_image.color.g, health_bar_image.color.b, health_bar_image.color.a - Time.unscaledDeltaTime * 1.0f);

            // UI effects
            cur_spark_dmg_time -= Time.deltaTime;
            if (cur_spark_dmg_time < 0)
                UI_dmg_sparks.enableEmission = false;
            else
                UI_dmg_sparks.enableEmission = true;

            cur_spark_healing_time -= Time.deltaTime;
            if (cur_spark_healing_time < 0)
                UI_healing_sparks.enableEmission = false;
            else
                UI_healing_sparks.enableEmission = true;

            cur_spark_transfer_time -= Time.deltaTime;
            if (cur_spark_transfer_time < 0)
                UI_transfer_sparks.enableEmission = false;
            else
                UI_transfer_sparks.enableEmission = true;
        }
    }
    void FixedUpdate()
    {
        // ROTATION
        // Lerp to our desired rotation
        physics.MoveRotation(Mathf.Lerp(transform.eulerAngles.z, desired_rotation, rotation_changing_speed));

        // Ripple the grid behind the car

        if(wake_counter <= Time.time)
        {
            EffectsManager.effects.GridWake((Vector2)transform.position, grid_ripple_force, grid_ripple_radius, primary_colour, false);
            wake_counter = wake_cooldown + Time.time;
        }
        
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


    public void AdjustHealth(float amount)
    {
        // Health bar around the car
        Health = Mathf.Clamp(Health + amount, 0, Max_Health);
        health_bar_image.fillAmount = Health / Max_Health;
        health_bar_image.color = new Color(health_bar_image.color.r, health_bar_image.color.g, health_bar_image.color.b, 1f);

        // If at low health, turn on the low HP warning
        UI_low_hp_warning.SetActive(Health / Max_Health <= 0.25f);

        // Set health bar at top of UI
        InGameUIManager.ui_manager.UpdateHealth();
    }

    public void TakeHit(float damage, bool getting_hit_explosion_noise = false)
    {
        low_hp_red_circle.StartExpanding();

        float prev_health = Health;

        if (currently_boosting)
            damage = damage / 2f;

        if (damage != 0 && !GameState.game_state.game_over && !GameState.game_state.players_invuln && !GameState.game_state.debug_invulnerability)
            AdjustHealth(-damage);

        if (getting_hit_explosion_noise)
        {
            SoundMixer.sound_manager.PlayGettingHitExplosion();
        }
        else
        {
            SoundMixer.sound_manager.PlayGettingHit();
        }

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

        // Turn on damage sparks on the UI health bar
        cur_spark_dmg_time = spark_dmg_time;

        if (Health <= 0 && !GameState.game_state.game_over)
            Die();
    }

    public void Die()
    {
        alive = false;
        Health = 0;
        InGameUIManager.ui_manager.UpdateHealth();
        SoundMixer.sound_manager.Play8bitExplosion();
        ClearGrindingSparks();

        // Destroy tether
        if (Tether.tether != null)
            Tether.tether.DestroyTether();

        EffectsManager.effects.ViolentExplosion(this.transform.position);
        EffectsManager.effects.GridExplosion(this.transform.position, 2f, 9f, primary_colour);

        this.gameObject.layer = LayerMask.NameToLayer("Dead Player");
        this.gameObject.tag = "Obstacle";
        this.gameObject.AddComponent<PlayerDying>();

        GameState.game_state.Players.Remove(this);
        GameState.game_state.CheckGameOver(this);

        Destroy(this);
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


    /*
    void StayOnScreen()
    {
        Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minScreenBounds.x + screen_margins.x, maxScreenBounds.x - screen_margins.y),
            Mathf.Clamp(transform.position.y, minScreenBounds.y + screen_margins.x, maxScreenBounds.y - screen_margins.y),
            transform.position.z); 
    }*/


    // Shower of sparks on a collision!
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Bullet") || coll.gameObject.layer == LayerMask.NameToLayer("EnemyBossTether"))
            return;

        if (coll.gameObject.tag == "Player")// && coll.gameObject.GetComponent<PlayerController>().Health < this.Health)
        {
            TransferHealth(coll.gameObject);
        }

        // SPARKS
        // Show sparks on the side of the car it was hit on
        CollisionAt(coll.contacts[0].point);

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
        sparks.gameObject.transform.position = coll.contacts[0].point;

        if (!in_use_grinding_sparks.ContainsKey(coll.gameObject))
        {
            in_use_grinding_sparks.Add(coll.gameObject, sparks);
        }
        //sparks.GetComponent<TurnOffSparks>().time_remaining = sparks.GetComponent<TurnOffSparks>().start_time_remaining;

        // Only for spynet, needed for UPWARDS obstacles
        if (coll.gameObject.tag == "UpObstacle")
        {
            touching_forward_obstacle = true;
            cur_touching_forward_obstacle = touching_forward_obstacle_cooldown / 4f;
        }
    }
    // Show grinding sparks when touching another object
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject == null)
            return;

        if (coll.gameObject.tag == "Player")// && coll.gameObject.GetComponent<PlayerController>().Health < this.Health)
        {
            TransferHealth(coll.gameObject);
        }
        
        // SPARKS
        // Update the position of the grinding
        if (in_use_grinding_sparks.ContainsKey(coll.gameObject))// && in_use_grinding_sparks[coll.gameObject] != null)
        {
            ParticleSystem p = in_use_grinding_sparks[coll.gameObject];
            p.gameObject.transform.position = coll.contacts[0].point;
            p.GetComponent<TurnOffSparks>().StartSparks();
        }
        //else
        //  in_use_grinding_sparks.Remove(coll.gameObject);

        // Only for spynet, needed for UPWARDS obstacles
        if (coll.gameObject.tag == "UpObstacle")
        {
            touching_forward_obstacle = true;
            cur_touching_forward_obstacle = touching_forward_obstacle_cooldown / 4f;
        }
    }
    // Stop grinding against the object we were pushing against
    void OnCollisionExit2D(Collision2D coll)
    {
        // Do something about checking if object is dead
        if (in_use_grinding_sparks.ContainsKey(coll.gameObject))// && !coll.gameObject)
        {
            ParticleSystem sparks = in_use_grinding_sparks[coll.gameObject];
            sparks.GetComponent<TurnOffSparks>().StopSparks();
            in_use_grinding_sparks.Remove(coll.gameObject);
            free_grinding_sparks.Add(sparks);
        }
        else
        {

        }

        if (coll.gameObject.tag == "UpObstacle")
        {
            cur_touching_forward_obstacle = 0;
        }
    }
    
    void OnTriggerEnter2D(Collider2D coll)
    {
        // Check to see if we die touching a normal fast obstacle and the LEFT death zone
        if (coll.gameObject.layer == LayerMask.NameToLayer("Death Zone") 
            && ((GameState.game_state.tether_touching_obstacle || touching_forward_obstacle))
            && !coll.gameObject.name.Equals("UPDeathzone")
            )
        {
            HitDeathZone();
            return;
        }
        // Specifically for spynet, if tether is caught on UP obstacle
        else if (coll.gameObject.layer == LayerMask.NameToLayer("Death Zone")
            && ((GameState.game_state.tether_touching_obstacle_up) || touching_forward_obstacle)
            && coll.gameObject.name.Equals("UPDeathzone")
            )
        {
            // Maybe add a few frames of invulnerability? Like 4 or 5?
            HitDeathZone();
            return;
        }
        else
        {
            if ((coll.gameObject.layer == LayerMask.NameToLayer("Obstacles") || coll.gameObject.layer == LayerMask.NameToLayer("Slow Obstacles")) && !coll.gameObject.tag.Equals("UpObstacle"))
            {
                cur_touching_forward_obstacle = touching_forward_obstacle_cooldown;
                touching_forward_obstacle = true;
            }
        }
    }
    void OnTriggerStay2D(Collider2D coll)
    {
        if ((coll.gameObject.layer == LayerMask.NameToLayer("Obstacles") || coll.gameObject.layer == LayerMask.NameToLayer("Slow Obstacles")) && !coll.gameObject.tag.Equals("UpObstacle"))
        {
            cur_touching_forward_obstacle = touching_forward_obstacle_cooldown;
            touching_forward_obstacle = true;
        }

        // Check to see if we die
        if (coll.gameObject.layer == LayerMask.NameToLayer("Death Zone")
            && (GameState.game_state.tether_touching_obstacle || touching_forward_obstacle)
            && !coll.gameObject.name.Equals("UPDeathzone"))
        {
            HitDeathZone();
            return;
        }
        else if (coll.gameObject.layer == LayerMask.NameToLayer("Death Zone")
            && ((GameState.game_state.tether_touching_obstacle_up))
            && coll.gameObject.name.Equals("UPDeathzone")
            )
        {
            HitDeathZone();
            return;
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



    float last_health_transfer_lightning;

    // Equalize health between players
    public void TransferHealth(GameObject other_player)
    {
        if (GameState.game_state.can_transfer_health)
        {
            // Get other player
            PlayerController other_p = other_player.GetComponent<PlayerController>();

            if (other_p.Health < this.Health)
            {
                // Transfer health if we have health to give
                float health_transferred = Mathf.Min(Time.deltaTime * HP_transfer_rate, (this.Health - other_p.Health) / 2f);
                other_p.Heal(health_transferred);
                this.AdjustHealth(-health_transferred);

                cur_spark_transfer_time = spark_transfer_time;

                if (health_transferred > 0.1f)
                {
                    SoundMixer.sound_manager.PlayTransferHealth();

                    if (last_health_transfer_lightning + 0.03f < Time.time)
                    {
                        if (Tether.tether!=null)
                        {
                            TetherLightning.tether_lightning.RegularBolt(this.transform.position, other_player.transform.position, 0.6f, Color.green, 5);
                            last_health_transfer_lightning = Time.time;
                        }
                    }
                }
            }
        }
    }


    public void Heal(float amount)
    {
        cur_spark_healing_time = spark_healing_time;
        AdjustHealth(amount);
    }


    public void HitDeathZone()
    {
        this.Die();
        // Have other player die too, only in coop
        GameState.game_state.PlayerHitDeathzone();
    }


    void OnDestroy()
    {
        ClearGrindingSparks();
    }
}
