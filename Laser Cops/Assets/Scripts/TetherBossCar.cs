using UnityEngine;
using System.Collections;

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
    // Use this for initialization
    void Start () {
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

    void Update()
    {
        tether_lightning_cooldown -= Time.deltaTime;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!boosting && !stopped)
        {
            GetComponent<Rigidbody2D>().velocity = travelDirection.normalized * speed;
        }
        else if (boosting && !stopped)
        {
            GetComponent<Rigidbody2D>().velocity = travelDirection.normalized * 2* speed;
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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

    public void HitByTetherGraphics(Collision2D collision)
    {
        SoundMixer.sound_manager.PlaySyncopatedLazer();

        if (tether_lightning_cooldown <= 0)
        {
            tether_lightning_cooldown = 0.1f;
            //EffectsManager.effects.TetherDamageSparks(collision.contacts[0].point);
            TetherLightning.tether_lightning.BranchLightning(Tether.tether.GetRandomLink().transform.position, this.transform.position);
        }
    }

}
