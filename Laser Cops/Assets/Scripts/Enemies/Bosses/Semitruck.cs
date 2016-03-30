using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public List<Blink> up_arrows = new List<Blink>();
    public List<Blink> down_arrows = new List<Blink>();
    public List<ForwardShotScript> side_battery_1 = new List<ForwardShotScript>();
    public List<ForwardShotScript> side_battery_2 = new List<ForwardShotScript>();

    public List<string> queued_attacks = new List<string>();

    public bool performing_attack = false;

    void Awake ()
    {
        motor = joint.motor;
    }
	void Start ()
    {
        queued_attacks.Add("Opening");
        queued_attacks.Add("MoveUpFireLasersDown");
        queued_attacks.Add("MoveDownFireLasersUp");
        //StartCoroutine(Opening());
        //StartCoroutine(MoveUpFireLasersDown());
        //StartCoroutine("MoveUpFireLasersDown");
        //StartCoroutine(MoveDownFireLasersUp());
    }

    void Update()
    {
        truck_physics.MovePosition(truck.transform.position + cur_truck_speed * Time.deltaTime);
        //truck.transform.position += cur_truck_speed * Time.deltaTime;   

        if (!performing_attack)
        {
            if (queued_attacks.Count > 0)
            {
                // Begin our next attack
                StartCoroutine(queued_attacks[0]);
                queued_attacks.RemoveAt(0);
                performing_attack = true;
            }
            else
            {
                // Out of attacks, choose randomly
                queued_attacks.Add("MoveUpFireLasersDown");
            }
        }
    }




    // ATTACKS
    public IEnumerator Opening()
    {
        // Play honking noise

        // Have truck move up
        //truck_physics.velocity = new Vector2(30f, 0);
        StartMoving(new Vector2(20.0f, 0));
        while (truck.transform.position.x < 11f)
        {
            yield return new WaitForSeconds(0.1f);
        }
        StopMoving();

        performing_attack = false;
        yield return null;
    }
    
    // Move up (up, fire lazers downwards, players dodge left/right
    public IEnumerator MoveUpFireLasersDown()
    {
        SignalLeft();
        yield return new WaitForSeconds(1f);

        RotateSideGuns(false);

        // Move up
        StartMoving(new Vector2(0f, 20f));
        while (truck.transform.position.y < 5f)
        {
            yield return new WaitForSeconds(0.01f);
        }
        StopMoving();

        // Move right
        StartMoving(new Vector2(20f, 0f));
        while (truck.transform.position.x < 18f)
        {
            yield return new WaitForSeconds(0.01f);
        }
        StopMoving();

        // Alternate shooting batteries
        float wait_time = 1.5f;
        ShootBattery1();
        yield return new WaitForSeconds(wait_time);
        ShootBattery2();
        yield return new WaitForSeconds(wait_time);
        ShootBattery1();
        yield return new WaitForSeconds(wait_time);
        ShootBattery2();
        yield return new WaitForSeconds(wait_time);
        /*
        ShootBattery1();
        yield return new WaitForSeconds(wait_time);
        ShootBattery2();
        yield return new WaitForSeconds(wait_time);
        ShootBattery1();
        yield return new WaitForSeconds(wait_time);
        ShootBattery2();
        yield return new WaitForSeconds(wait_time);
        ShootBattery1();
        yield return new WaitForSeconds(wait_time);
        ShootBattery2();
        yield return new WaitForSeconds(wait_time);
        */

        // Add in extra gun batteries on hard mode


        // Move left
        StartMoving(new Vector2(-20f, 0f));
        while (truck.transform.position.x > 7f)
        {
            yield return new WaitForSeconds(0.01f);
        }
        StopMoving();

        performing_attack = false;
        yield return null;
    }
    // Move up (up, fire lazers downwards, players dodge left/right
    public IEnumerator MoveDownFireLasersUp()
    {
        SignalRight();
        yield return new WaitForSeconds(1f);

        RotateSideGuns(true);

        // Move down
        StartMoving(new Vector2(0f, -20f));
        while (truck.transform.position.y > -5f)
        {
            yield return new WaitForSeconds(0.01f);
        }
        StopMoving();

        // Move right
        StartMoving(new Vector2(20f, 0f));
        while (truck.transform.position.x < 18f)
        {
            yield return new WaitForSeconds(0.01f);
        }
        StopMoving();

        // Alternate shooting batteries
        float wait_time = 1.5f;
        ShootBattery1();
        yield return new WaitForSeconds(wait_time);
        ShootBattery2();
        yield return new WaitForSeconds(wait_time);
        ShootBattery1();
        yield return new WaitForSeconds(wait_time);
        ShootBattery2();
        yield return new WaitForSeconds(wait_time);
        /*
        ShootBattery1();
        yield return new WaitForSeconds(wait_time);
        ShootBattery2();
        yield return new WaitForSeconds(wait_time);
        ShootBattery1();
        yield return new WaitForSeconds(wait_time);
        ShootBattery2();
        yield return new WaitForSeconds(wait_time);
        ShootBattery1();
        yield return new WaitForSeconds(wait_time);
        ShootBattery2();
        yield return new WaitForSeconds(wait_time);
        */
        // Add in extra gun batteries on hard mode

        // Move left
        StartMoving(new Vector2(-20f, 0f));
        while (truck.transform.position.x > 7f)
        {
            yield return new WaitForSeconds(0.01f);
        }
        StopMoving();

        performing_attack = false;
        yield return null;
    }




    // END ATTACKS



    public void StartMoving(Vector2 speed)
    {
        if (Mathf.Abs(speed.x) > 0 && (Mathf.Abs(speed.y) > 0))
        {
            truck_physics.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            if (Mathf.Abs(speed.x) > 0)
            {
                truck_physics.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            }
            if (Mathf.Abs(speed.y) > 0)
            {
                truck_physics.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
        }

        cur_truck_speed = speed;
    }

    public void StopMoving()
    {
        cur_truck_speed = Vector3.zero;
        truck_physics.velocity = Vector2.zero;
        motor.motorSpeed = 0f;
        truck_constraints = RigidbodyConstraints2D.FreezePosition;
    }
    public void SignalLeft()
    {
        foreach (Blink b in up_arrows)
        {
            b.StartBlinking(3.0f, 0.5f);
        }
    }
    public void SignalRight()
    {
        foreach (Blink b in down_arrows)
        {
            b.StartBlinking(3.0f, 0.5f);
        }
    }

    public void RotateSideGuns(bool up)
    {
        float angle = 0;
        if (up)
            angle = 180;

        foreach (ForwardShotScript s in side_battery_1)
        {
            s.transform.eulerAngles = new Vector3(0, 0, angle);
            //s.transform.rotation.Set(0, 0, angle, 0);
        }
        foreach (ForwardShotScript s in side_battery_2)
        {
            s.transform.eulerAngles = new Vector3(0, 0, angle);
            //s.transform.rotation.Set(0, 0, angle, 0);
        }
    }

    public void ShootBattery1()
    {
        foreach (ForwardShotScript s in side_battery_1)
        {
            s.shoot();
        }
    }
    public void ShootBattery2()
    {
        foreach (ForwardShotScript s in side_battery_2)
        {
            s.shoot();
        }
    }
}
