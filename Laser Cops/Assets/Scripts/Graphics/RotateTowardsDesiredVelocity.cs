using UnityEngine;
using System.Collections;

// Used by enemies to rotate their wheels in the right direction
public class RotateTowardsDesiredVelocity : MonoBehaviour
{
    basicScrollingEnemyScript player;
    float default_rotation; // Rotation we return to if doing nothing
    float rotation_changing_speed = 0.04f;   // How quickly we lerp between rotations
    float wheel_offset = 40f;
    //float max_turning_rotation = 20f;
    //float sideways_turning_speed = 100.0f;

    void Start()
    {
        default_rotation = transform.rotation.eulerAngles.z;
        player = transform.root.GetComponent<basicScrollingEnemyScript>();

        if (!player)
            Destroy(this);
    }



    void LateUpdate()
    {
        // ROTATION
        // Lerp to our desired rotation
        float offset = 0;
        if (GameState.game_state.going_sideways)
        {
            if (player.desired_velocity.y > 0)
            {
                offset = wheel_offset;
            }
            else if (player.desired_velocity.y < 0)
            {
                offset = -wheel_offset;
            }
        }
        else
        {
            if (player.desired_velocity.x > 0)
            {
                offset = wheel_offset;
            }
            else if (player.desired_velocity.x < 0)
            {
                offset = -wheel_offset;
            }
        }
        this.transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(this.transform.eulerAngles.z, default_rotation + offset, rotation_changing_speed));
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
