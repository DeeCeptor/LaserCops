using UnityEngine;
using System.Collections;

public class TetherClamping : MonoBehaviour
{
    Transform player_1;
    Transform player_2;


    void Start ()
    {
        player_1 = GameState.game_state.PlayerObjects[0].transform;
        player_2 = GameState.game_state.PlayerObjects[1].transform;
    }


    void Update ()
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
	}


    public void TouchedObstacle()
    {
        GameState.game_state.tether_touching_obstacle = true;
        GameState.game_state.time_last_touched_obstacle = Time.time;
        GameState.game_state.SetVelocityPositionIterations(GameState.intensive_velocity_iterations, GameState.intensive_position_iterations);
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            TouchedObstacle();
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
