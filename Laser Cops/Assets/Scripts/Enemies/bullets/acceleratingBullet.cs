using UnityEngine;
using System.Collections;

public class acceleratingBullet : BulletScript {
	
	// Update is called once per frame
	void FixedUpdate () {
        GetComponent<Rigidbody2D>().AddForce( dir.normalized * speed);
    }
}
