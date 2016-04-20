using UnityEngine;
using System.Collections;

public class HomingBulletScript : MonoBehaviour {
    public float speed = 2;
    private Transform playerToTrack;
    private GameObject[] players;
    public float damage = 10f;
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
        GetComponent<Rigidbody2D>().velocity = -transform.up * speed;
        Vector3 vectorToTarget = playerToTrack.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime/trackingPower);
    }

    public void CheckDeath()
    {
        if (!GetComponent<SpriteRenderer>().isVisible)
        {
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.tag == "Player")
        {
            // Spawn small sparks and explosion
            EffectsManager.effects.BulletHitPlayer(collision.contacts[0].point);

            collision.gameObject.GetComponent<PlayerController>().TakeHit(damage);
        }

        else if (collision.gameObject.tag == "VIP")
        {
            collision.gameObject.GetComponent<VIPScript>().TakeHit(damage);
        }

        else if (!collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
