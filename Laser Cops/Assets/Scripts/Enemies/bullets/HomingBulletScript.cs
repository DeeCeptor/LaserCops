using UnityEngine;
using System.Collections;

public class HomingBulletScript : BulletScript
{
    private Transform playerToTrack;
    private GameObject[] players;
    //the LOWER this is the faster it will track
    public float trackingPower = 3f;


    void Start ()
    {
        players = GameState.game_state.PlayerObjects;
        int randInt = Random.Range(0, players.Length);
        playerToTrack = players[randInt].transform;
    }


    void FixedUpdate ()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;

        Vector3 vectorToTarget;
        if (!reflected_bullet)
            vectorToTarget = playerToTrack.position - transform.position;
        else
            vectorToTarget = transform.position - playerToTrack.position;

        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle-90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime/trackingPower);
    }
}
