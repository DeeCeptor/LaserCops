using UnityEngine;
using System.Collections;

public class bounceBomb : MonoBehaviour {

    public float damageToBoss = 10f;
    public float damageToPlayers = 15f;
    public float timeTillDetonation = 20f;
    public float timeTillDetonationEasy = 30f;
    public float timeTillDetonationHard = 15f;
    public float countDownTimer;
    public TextMesh countDownTimerText;
    //the x Co-ordinate of the left of screen set at the start
    public float xLeftOfScreen;
    public bool inactive;

    //the game Object that spawns the bounce bomb
    public GameObject spawner;

    //speed bomb will move towards center
    public float centerAttraction = 0.01f;

	// Use this for initialization
	void Start () {
        //set timer based on difficulty
        if (GameState.game_state.current_difficulty == GameState.Difficulty.Easy)
        {
            countDownTimer = Time.time + timeTillDetonationEasy;
        }
        if (GameState.game_state.current_difficulty == GameState.Difficulty.Normal)
        {
            countDownTimer = Time.time + timeTillDetonation;
        }
        if (GameState.game_state.current_difficulty == GameState.Difficulty.Hard)
        {
            countDownTimer = Time.time + timeTillDetonationHard;
        }
        countDownTimerText.text = (countDownTimer - Time.time).ToString();

        //set left side of screen
        xLeftOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(0,0,0)).x + GetComponent<SpriteRenderer>().bounds.size.x;
        
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 12)
        {
            gameObject.layer = 25;
        }

        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().TakeHit(damageToPlayers, true);
            EffectsManager.effects.ViolentExplosion(collision.contacts[0].point);
            EffectsManager.effects.TetherGrindSparks(collision.contacts[0].point);
            EffectsManager.effects.BurstLargeFireball(collision.contacts[0].point);
            SoundMixer.sound_manager.PlayGettingHitExplosion();
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        countDownTimerText.text = ((int)(countDownTimer - Time.time)).ToString();
        if(countDownTimer < Time.time)
        {
            Detonate();
        }
        if(transform.position.x <= xLeftOfScreen)
        {
            Destroy(gameObject);
        }
	}

    public void Detonate()
    {
        EffectsManager.effects.ViolentExplosion(transform.position);
        EffectsManager.effects.TetherGrindSparks(transform.position);
        EffectsManager.effects.BurstLargeFireball(transform.position);
        SoundMixer.sound_manager.PlayGettingHitExplosion();
        Destroy(gameObject);
    }
}
