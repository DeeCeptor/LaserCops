using UnityEngine;
using System.Collections;

public class Semitruck : MonoBehaviour
{
    public HingeJoint2D joint;
    JointMotor2D motor;
    public GameObject truck;
    RigidbodyConstraints2D truck_constraints;
    public Rigidbody2D truck_physics;
    public GameObject trailer;
    public Rigidbody2D trailer_physics;

    public Vector3 cur_truck_speed;

    void Awake ()
    {
        motor = joint.motor;
    }
	void Start ()
    {
        StartCoroutine(Opening());
	}
	
	void Update ()
    {
        truck_physics.MovePosition(truck.transform.position + cur_truck_speed * Time.deltaTime);
        //truck.transform.position += cur_truck_speed * Time.deltaTime;   
	}


    public IEnumerator Opening()
    {
        // Play honking noise

        // Have truck move up
        //truck_physics.velocity = new Vector2(30f, 0);
        cur_truck_speed = new Vector3(20.0f, 0, 0);
        while (truck.transform.position.x < 11f)
        {
            yield return new WaitForSeconds(0.1f);
        }
        StopMoving();

        // Pick our next attack

        yield return null;
    }

    public void StopMoving()
    {
        cur_truck_speed = Vector3.zero;
        truck_physics.velocity = Vector2.zero;
        motor.motorSpeed = 0f;
        //truck_constraints = RigidbodyConstraints2D.FreezePosition;
    }
    
}
