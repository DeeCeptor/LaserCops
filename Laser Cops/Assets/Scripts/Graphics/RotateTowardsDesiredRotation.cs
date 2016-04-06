using UnityEngine;
using System.Collections;

public class RotateTowardsDesiredRotation : MonoBehaviour
{
    PlayerController player;
    Rigidbody2D physics;
    float default_rotation; // Rotation we return to if doing nothing
    float rotation_changing_speed = 0.5f;   // How quickly we lerp between rotations
    //float max_turning_rotation = 20f;
    //float sideways_turning_speed = 100.0f;

    void Start()
    {
        default_rotation = transform.rotation.eulerAngles.z;
        physics = this.GetComponent<Rigidbody2D>();
        player = transform.root.GetComponent<PlayerController>();
    }



    void LateUpdate()
    {
        /*
        // Animate car turning
        // Animate breaking/accelerating forwards
        if (GameState.game_state.going_sideways)
        {
            TurningCar(player.desired_rotation);
        }
        else
        {
            TurningCar(player.desired_rotation);
        }*/


        // ROTATION
        // Lerp to our desired rotation
        this.transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(this.transform.eulerAngles.z, player.desired_rotation, rotation_changing_speed));
    }
    /*
    // Slightly rotate car to make it look like turning
    public void TurningCar(float amount)
    {
        if (amount > 0)
        {
            // Sideways: going left
            // Turn more as we keep turning
            desired_rotation = Mathf.Min(desired_rotation + (Time.deltaTime * sideways_turning_speed), default_rotation + max_turning_rotation);
            rotation_changing_speed = 0.2f;
        }
        else if (amount < 0)
        {
            // Sideways: going right
            desired_rotation = Mathf.Max(desired_rotation - (sideways_turning_speed * Time.deltaTime), default_rotation - max_turning_rotation);
            rotation_changing_speed = 0.2f;
        }
        else
        {
            // Not turning, return to normal rotation
            desired_rotation = default_rotation;
            rotation_changing_speed = 0.05f;
        }
    }*/
}
