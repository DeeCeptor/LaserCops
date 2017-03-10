using UnityEngine;
using System.Collections;

public class OrbitAround : MonoBehaviour
{

    GameObject cube;
    public Transform center;
    public Vector3 center_pos;
    public Vector3 axis = Vector3.up;
    public Vector3 desiredPosition;
    public float radius = 2.0f;
    public float radiusSpeed = 0.5f;
    public float rotationSpeed = 80.0f;

    public bool randomize_attributes = false;

    void Start()
    {
        if (randomize_attributes)
        {
            radius = Random.Range(0.1f, 0.9f);
            radiusSpeed = Random.Range(0.1f, 0.6f);
            rotationSpeed = Random.Range(20, 80);
        }

        center_pos = center.position;
        center_pos.y += 0.5f;

        transform.position = (transform.position - center_pos).normalized * radius + center_pos;
        //radius = 2.0f;
    }

    void Update()
    {
        transform.RotateAround(center_pos, axis, rotationSpeed * Time.deltaTime);
        desiredPosition = (transform.position - center_pos).normalized * radius + center_pos;
        transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
    }
}