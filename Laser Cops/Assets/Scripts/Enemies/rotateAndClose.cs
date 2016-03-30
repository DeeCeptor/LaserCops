using UnityEngine;
using System.Collections;

public class rotateAndClose : basicArenaEnemy {
    public bool rotateClockwise = false;
    public float rotationSpeed = 2f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!active)
        {
            CheckActive();
            moveInactive();
        }
        else
        {
            Follow();
            RotateAround();
            CheckDeath();
        }
    }

    public void RotateAround()
    {
        Vector3 vectorToTarget = playerToTrack.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);

        if(!rotateClockwise)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + (Vector2)(transform.up * rotationSpeed);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + (Vector2)(-transform.up * rotationSpeed);
        }
    }
}
