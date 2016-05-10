using UnityEngine;
using System.Collections;

// http://answers.unity3d.com/questions/1032656/trail-renderer-on-a-non-moving-object.html
public class ManualTrail : MonoBehaviour
{
    public int trailResolution;     // Number of points on the line
    LineRenderer lineRenderer;

    Vector3[] lineSegmentPositions;
    Vector3[] lineSegmentVelocities;
    public float startWidth = 1f;
    public float endWidth = 1f;
    public Color startColor;
    public Color endColor;
    // This would be the distance between the individual points of the line renderer
    public float offset;
    Vector3 facingDirection;

    public enum LocalDirections { FlyingLeft, FlyingRight, FlyingDown, FlyingUp }
    public LocalDirections localDirectionToUse;

    // How far the points 'lag' behind each other in terms of position
    public float lagTime;

    Vector3 GetDirection()
    {
        switch (localDirectionToUse)
        {
            case LocalDirections.FlyingLeft:
                return transform.right;
            case LocalDirections.FlyingRight:
                return -transform.right;
            case LocalDirections.FlyingDown:
                return transform.up;
            case LocalDirections.FlyingUp:
                return -transform.up;
        }

        Debug.LogError("The variable 'localDirectionToUse' on the 'ManualTrail' script, located on object " + name + ", was somehow invalid. Please investigate!");
        return Vector3.zero;
    }

    // Use this for initialization
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.SetVertexCount(trailResolution);
        lineRenderer.SetWidth(startWidth, endWidth);
        lineRenderer.SetColors(startColor, endColor);
        lineSegmentPositions = new Vector3[trailResolution];
        lineSegmentVelocities = new Vector3[trailResolution];

        facingDirection = GetDirection();

        // Initialize our positions
        for (int i = 0; i < lineSegmentPositions.Length; i++)
        {
            lineSegmentPositions[i] = new Vector3();
            lineSegmentVelocities[i] = new Vector3();

            if (i == 0)
            {
                // Set the first position to be at the base of the transform
                lineSegmentPositions[i] = transform.position;
            }
            else
            {
                // All subsequent positions would be an offset of the original position.
                lineSegmentPositions[i] = transform.position + (facingDirection * (offset * i));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        facingDirection = GetDirection();

        for (int i = 0; i < lineSegmentPositions.Length; i++)
        {
            if (i == 0)
            {
                // We always want the first position to be exactly at the original position
                lineSegmentPositions[i] = transform.position;
            }
            else
            {
                // All others will follow the original with the offset that you set up
                lineSegmentPositions[i] = Vector3.SmoothDamp(lineSegmentPositions[i], lineSegmentPositions[i - 1] + (facingDirection * offset), ref lineSegmentVelocities[i], lagTime);
            }

            // Once we're done calculating where our position should be, set the line segment to be in its proper place
            lineRenderer.SetPosition(i, lineSegmentPositions[i]);
        }
    }
}