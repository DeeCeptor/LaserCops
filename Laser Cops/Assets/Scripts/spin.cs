using UnityEngine;
using System.Collections;

public class spin : MonoBehaviour {
    //a maximum amount of offset. A higher number means the spin will have a more random velocity. This only applies to dimensions it is rotating at more than 0 speed in
    public float randomOffset = 0;
    public Vector3 rotationSpeed;
	// Use this for initialization
	void Start () {
        if (randomOffset > 0)
        {
            if (rotationSpeed.x > 0)
            {
                rotationSpeed.x = rotationSpeed.x + Random.Range(0, randomOffset);
            }
            if (rotationSpeed.y > 0)
            {
                rotationSpeed.y = rotationSpeed.y + Random.Range(0, randomOffset);
            }
            if (rotationSpeed.z > 0)
            {
                rotationSpeed.z = rotationSpeed.z + Random.Range(0, randomOffset);
            }
            transform.rotation = new Quaternion(Random.rotation.x,Random.rotation.y,0,0);
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        
        transform.Rotate(rotationSpeed);

	}
}
