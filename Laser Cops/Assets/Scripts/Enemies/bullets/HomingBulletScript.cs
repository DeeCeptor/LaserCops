using UnityEngine;
using System.Collections;

public class HomingBulletScript : BulletScript {
    private Transform playerToTrack;
    private GameObject[] players;
    //the LOWER this is the faster it will track
    public float trackingPower = 3f;
    // Use this for initialization
    void Start () {
        players = GameState.game_state.PlayerObjects;
        int randInt = Random.Range(0, players.Length);
        playerToTrack = players[randInt].transform;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;
        Vector3 vectorToTarget = playerToTrack.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle-90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime/trackingPower);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.tag == "Player")
        {
            // Spawn small sparks and explosion
            EffectsManager.effects.BulletHitPlayer(collision.contacts[0].point);

            collision.gameObject.GetComponent<PlayerController>().TakeHit(damage, true);
        }

        else if (collision.gameObject.tag == "VIP")
        {
            collision.gameObject.GetComponent<VIPScript>().TakeHit(damage);
        }

        if (!collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }
}
