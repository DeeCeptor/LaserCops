using UnityEngine;
using System.Collections;

// Damages enemies when boosting
public class BoostingBumper : MonoBehaviour
{
    PlayerController p;

    void Start()
    {
        p = this.transform.root.GetComponent<PlayerController>();
    }


    void OnTriggerEnter2D(Collider2D coll)
    {
        // Damage enemy
        if (p.currently_boosting && coll.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            if (coll.gameObject.GetComponent<basicScrollingEnemyScript>() != null)
                coll.gameObject.GetComponent<basicScrollingEnemyScript>().TakeHit(p.boosting_damage);
            else if (coll.gameObject.GetComponent<basicArenaEnemy>() != null)
                coll.gameObject.GetComponent<basicArenaEnemy>().TakeHit(p.boosting_damage);
        }
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        // Damage enemy
        if (p.currently_boosting && coll.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            if (coll.gameObject.GetComponent<basicScrollingEnemyScript>() != null)
                coll.gameObject.GetComponent<basicScrollingEnemyScript>().TakeHit(p.boosting_damage);
            else if (coll.gameObject.GetComponent<basicArenaEnemy>() != null)
                coll.gameObject.GetComponent<basicArenaEnemy>().TakeHit(p.boosting_damage);
        }
    }
}
