using UnityEngine;
using System.Collections;

public class EnemyDying : MonoBehaviour
{
    Rigidbody2D physics;
    float time_left = 10f;
    public float rotation_speed = 300f;
    float direction;
    float force_min = 50f;
    float force_max = 300f;

    void Awake ()
    {
        physics = this.GetComponent<Rigidbody2D>();

        GameObject obj = (GameObject)Instantiate((GameObject)Resources.Load("Graphics/Fireball") as GameObject, transform.position, Quaternion.identity);
        obj.transform.parent = this.gameObject.transform;
    }
    void Start ()
    {

    }

    // -1 is up, 1 is down
    public void JustDied(float new_direction)
    {
        direction = Mathf.Sign(new_direction);
        rotation_speed = direction * rotation_speed;

        // Add force in that direction
        physics.AddForce(new Vector2(Random.Range(force_min, force_max), direction * Random.Range(0, force_max)));

        this.gameObject.AddComponent<PolygonCollider2D>();
    }


    void Update ()
    {
        // Constantly rotate
        physics.MoveRotation(this.physics.rotation + Time.deltaTime * rotation_speed);

        time_left -= Time.deltaTime;
        if (time_left <= 0)
            Destroy(this.gameObject);
    }
}
