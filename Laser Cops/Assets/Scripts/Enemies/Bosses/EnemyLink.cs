using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class EnemyLink : MonoBehaviour
{
    public GameObject above;
    public GameObject below;

    public int position_from_top_in_rope;    // Where 0 is the top
    public int position_from_bottom_in_rope;    // Where 0 is the botom
    public GameObject top_most;     // Top of the rope
    public GameObject bottom_most;  // Bottom of the rope

    public List<GameObject> all_segments;

    public EnemyBossTetherScript rope;

    public float collisionDamage = 1.0f;
    public float hardCollisionDamage = 1.0f;
    public float easyCollisionDamage = 1.0f;

    void Start()
    {
        if (GameState.game_state.current_difficulty == GameState.Difficulty.Hard)
        {
            collisionDamage = hardCollisionDamage;
        }

        else if (GameState.game_state.current_difficulty == GameState.Difficulty.Easy)
        {
            collisionDamage = easyCollisionDamage;
        }

    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        ResolveCollision(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        ResolveCollision(collision);
    }

    void ResolveCollision(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Hurt the player
            collision.gameObject.GetComponent<PlayerController>().TakeHit(collisionDamage);

            if (BonnieAndClydeBehaviour.player_lightning_cur_cooldown <= 0f)
            {
                // Spawn red lightning
                TetherLightning.tether_lightning.BranchLightning(EnemyBossTetherScript.EnemyTether.GetRandomLink().transform.position, collision.transform.position);
                BonnieAndClydeBehaviour.player_lightning_cur_cooldown = BonnieAndClydeBehaviour.player_lightning_cooldown;
            }
        }
    }
}
