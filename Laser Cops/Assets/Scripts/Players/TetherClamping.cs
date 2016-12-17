using UnityEngine;
using System.Collections;

public class TetherClamping : MonoBehaviour
{
    Transform player_1;
    Transform player_2;


    void Start ()
    {
        /*
        player_1 = GameState.game_state.PlayerObjects[0].transform;
        player_2 = GameState.game_state.PlayerObjects[1].transform;*/
        player_1 = GameState.game_state.Players[0].transform;
        player_2 = GameState.game_state.Players[1].transform;
    }


    void Update ()
    {
        /*
        if (!GameState.game_state.chained_to_center && GameState.game_state.PlayerObjects.Length <= 2)
        {
            if (player_1.transform.position.y < player_2.transform.position.y)
            {
                this.transform.position = new Vector3(
                    transform.position.x,//Mathf.Clamp(this.transform.position.x, player_1.transform.position.x, player_2.transform.position.x), 
                    Mathf.Clamp(this.transform.position.y, player_1.transform.position.y, player_2.transform.position.y));
            }
            else
            {
                this.transform.position = new Vector3(
                    transform.position.x,//Mathf.Clamp(this.transform.position.x, player_1.transform.position.x, player_2.transform.position.x), 
                    Mathf.Clamp(this.transform.position.y, player_2.transform.position.y, player_1.transform.position.y));
            }
        }*/
	}


    public void TouchedObstacle()
    {
        GameState.game_state.tether_touching_obstacle = true;
        GameState.game_state.time_last_touched_obstacle = Time.time;
        GameState.game_state.SetVelocityPositionIterations(GameState.intensive_velocity_iterations, GameState.intensive_position_iterations);
    }
    public void LimitPlayerMovement()
    {
        GameState.game_state.limit_player_control_from_obstacles = true;
    }


    public void AbsorbBullet(GameObject bullet)
    {
        GameState.game_state.Heal_All_Players(3f);
        InGameUIManager.ui_manager.ChangeScore(2, this.transform.position);
        Destroy(bullet);
    }
    // Reverse direction of bullet
    public void ReflectBullet(GameObject bullet)
    {
        // Invert direction
        //bullet.GetComponent<Rigidbody2D>().velocity = bullet.GetComponent<Rigidbody2D>().velocity * -4f;
        //bullet.GetComponent<BulletScript>().dir *= -1;
        bullet.GetComponent<BulletScript>().speed *= -4;

        // Set new layer
        bullet.gameObject.layer = LayerMask.NameToLayer("ReboundingBullet");

        // Set damage
        bullet.GetComponent<BulletScript>().damage *= 3;
        bullet.GetComponent<BulletScript>().reflected_bullet = true;

        // Change looks and sprite
        bullet.GetComponent<SpriteRenderer>().color = Color.green;
        bullet.GetComponent<Rigidbody2D>().angularVelocity = 0;
        bullet.GetComponent<SpriteRenderer>().material = GameState.game_state.default_sprite_material;

        SoundMixer.sound_manager.PlayShortSpark();

        EffectsManager.effects.BulletReflected(this.transform.position);
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            TouchedObstacle();
            LimitPlayerMovement();
        }
        else if (coll.gameObject.layer == LayerMask.NameToLayer("Slow Obstacles"))
        {
            TouchedObstacle();
        }
        else if (Tether.tether.cur_tether_mode == Tether.TetherMode.Capture && coll.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            ReflectBullet(coll.gameObject);
            //AbsorbBullet(coll.gameObject);
        }
    }
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            TouchedObstacle();
            LimitPlayerMovement();
        }
        else if (coll.gameObject.layer == LayerMask.NameToLayer("Slow Obstacles"))
        {
            TouchedObstacle();
        }
    }
    void OnCollisionExit2D(Collision2D coll)
    {

    }
}
