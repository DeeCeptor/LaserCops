using UnityEngine;
using System.Collections.Generic;

//note that the parent object has a script which will control bonny and clyde this script is to be attached to the cars themselves

public class TetherBossCar : MonoBehaviour {

    public Vector2 travelDirection;
    public BossHealthScript BonnyAndClydeHealth;
    public float speed = 1f;
    public bool stopped = false;
    public bool boosting = false;

    protected float tether_lightning_cooldown;

    public float yTopOfScreen = 0f;
    public float yBottomOfScreen = 0f;
    public float xRightOfScreen = 0f;
    public float xLeftOfScreen = 0f;

    GameObject highway;
    BoxCollider2D box;
    Rigidbody2D rigidbody;

    public List<ParticleSystem> moving_forward_particles = new List<ParticleSystem>();
    public List<ParticleSystem> moving_backwards_particles = new List<ParticleSystem>();


    void Awake ()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
    }
    void Start ()
    {
        BonnyAndClydeHealth = GetComponentInParent<BossHealthScript>();

        //start by traveling left
        travelDirection = new Vector2(-1,0);

        //get the bound bonnie and clyde need to stay in
        box = this.GetComponent<BoxCollider2D>();
        highway = GameObject.FindGameObjectWithTag("Grid");
        MeshRenderer mesh = highway.GetComponent<MeshRenderer>();

        float screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - box.bounds.extents.x;
        float screenLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + box.bounds.extents.x;

        Vector3 minScreenBounds = new Vector3(screenLeft, mesh.bounds.min.y + box.bounds.extents.y, 0);
        Vector3 maxScreenBounds = new Vector3(screenRight, mesh.bounds.max.y - box.bounds.extents.y, 0);

        yTopOfScreen = maxScreenBounds.y;
        yBottomOfScreen = minScreenBounds.y;
        xRightOfScreen = maxScreenBounds.x;
        xLeftOfScreen = minScreenBounds.x;
    }


    void FixedUpdate ()
    {
        if (!boosting && !stopped)
        {
            rigidbody.velocity = travelDirection.normalized * speed;
        }
        else if (boosting && !stopped)
        {
            rigidbody.velocity = travelDirection.normalized * 2* speed;
        }
        else
        {
            rigidbody.velocity = Vector2.zero;
        }

        if (transform.position.x <= xLeftOfScreen&&travelDirection.x<0)
        {
            travelDirection.x = -travelDirection.x;
        }

        else if (transform.position.x >= xRightOfScreen && travelDirection.x > 0)
        {
            travelDirection.x = -travelDirection.x;
        }

        if (transform.position.y <= yBottomOfScreen && travelDirection.y < 0)
        {
            travelDirection.y = -travelDirection.y;
        }
        else if (transform.position.y >= yTopOfScreen && travelDirection.y > 0)
        {
            travelDirection.y = -travelDirection.y;
        }

        if(BonnyAndClydeHealth.health <=0)
        {
            Die();
        }

        if (travelDirection.x > 0)
        {
            MovingForwardGraphics();
        }
        else if (travelDirection.x < 0)
        {
            MovingBackwardsGraphics();
        }
    }


    public void MovingForwardGraphics()
    {
        // Turn on forward thruster
        foreach (ParticleSystem ps in moving_forward_particles)
        {
            if (!ps.isPlaying)
                ps.Play();
        }
        foreach (ParticleSystem ps in moving_backwards_particles)
        {
            if (ps.isPlaying)
                ps.Stop();
        }
    }
    public void MovingBackwardsGraphics()
    {
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



    void Update()
    {
        tether_lightning_cooldown -= Time.deltaTime;
    }


    public void Die()
    {
        SoundMixer.sound_manager.Play8bitExplosion();
        EffectsManager.effects.ViolentExplosion(this.transform.position);
        EffectsManager.effects.GridExplosion(this.transform.position, 2f, 8f, Color.red);

        GameObject[] corpses = EffectsManager.effects.CutSprite(this.gameObject);

        Destroy(gameObject);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 12 && BonnyAndClydeHealth.hurtByTether == true && BonnyAndClydeHealth.hit == false)
        {
            if (GameState.game_state.current_difficulty == GameState.Difficulty.Hard)
            {
                BonnyAndClydeHealth.takeHit(Tether.tether.Damage * BonnyAndClydeHealth.hardDamageMultiplyer);
            }
            else if (GameState.game_state.current_difficulty == GameState.Difficulty.Easy)
            {
                BonnyAndClydeHealth.takeHit(Tether.tether.Damage * BonnyAndClydeHealth.easyDamageMultiplyer);
            }
            else
            {
                BonnyAndClydeHealth.takeHit(Tether.tether.Damage);
            }
            HitByTetherGraphics(collision);
            BonnyAndClydeHealth.hit = true;
        }

    }


    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 12 && BonnyAndClydeHealth.hurtByTether == true && BonnyAndClydeHealth.hit == false)
        {
            if (GameState.game_state.current_difficulty == GameState.Difficulty.Hard)
            {
                BonnyAndClydeHealth.takeHit(Tether.tether.Damage * BonnyAndClydeHealth.hardDamageMultiplyer);
            }
            else if (GameState.game_state.current_difficulty == GameState.Difficulty.Easy)
            {
                BonnyAndClydeHealth.takeHit(Tether.tether.Damage * BonnyAndClydeHealth.easyDamageMultiplyer);
            }
            else
            {
                BonnyAndClydeHealth.takeHit(Tether.tether.Damage);
            }
            HitByTetherGraphics(collision);
            BonnyAndClydeHealth.hit = true;
        }
    }


    public void HitByTetherGraphics(Collision2D collision)
    {
        SoundMixer.sound_manager.PlaySyncopatedLazer();

        if (tether_lightning_cooldown <= 0)
        {
            tether_lightning_cooldown = 0.2f;
            //EffectsManager.effects.TetherDamageSparks(collision.contacts[0].point);
            TetherLightning.tether_lightning.BranchLightning(Tether.tether.GetRandomLink().transform.position, this.transform.position);
        }
    }

}
