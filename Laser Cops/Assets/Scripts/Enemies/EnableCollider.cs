using UnityEngine;
using System.Collections;

public class EnableCollider : MonoBehaviour
{
    float time_till_collider_enabled = 2f;

    void Awake()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;

        this.GetComponent<basicScrollingEnemyScript>().active = true;
    }


    void Update ()
    {
        time_till_collider_enabled -= Time.deltaTime;
        Debug.Log(time_till_collider_enabled);
        if (time_till_collider_enabled <= 0)
        {
            this.GetComponent<BoxCollider2D>().enabled = true;
            Destroy(this);
        }
    }
}
