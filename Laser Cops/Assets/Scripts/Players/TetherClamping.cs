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


    public void AbsorbBullet(GameObject bullet)
    {
        GameState.game_state.Heal_All_Players(3f);
        InGameUIManager.ui_manager.ChangeScore(2, this.transform.position);
        Destroy(bullet);
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            TouchedObstacle();
        }
        else if (Tether.tether.cur_tether_mode == Tether.TetherMode.Capture && coll.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            AbsorbBullet(coll.gameObject);
        }
    }
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            TouchedObstacle();
        }
    }
    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            //GameState.game_state.tether_touching_obstacle = false;
        }
    }
}
