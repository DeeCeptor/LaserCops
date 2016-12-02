using UnityEngine;
using System.Collections;

//note that the parent object has a script which will control bonny and clyde this script is to be attached to the cars themselves

public class TetherBossCar : MonoBehaviour {

    public Vector2 travelDirection;
    public BossHealthScript BonnyAndClydeHealth;
    public float speed = 1f;
    public bool stopped = false;
    public bool boosting = false;
	// Use this for initialization
	void Start () {
        BonnyAndClydeHealth = GetComponentInParent<BossHealthScript>();

        //start by traveling left
        travelDirection = new Vector2(-1,0);
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

        }
    }

    }
