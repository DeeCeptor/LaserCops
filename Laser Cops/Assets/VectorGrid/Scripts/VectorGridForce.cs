using UnityEngine;
using System.Collections;

public class VectorGridForce : MonoBehaviour 
{
    [HideInInspector]
	public VectorGrid m_VectorGrid;
	public float m_ForceScale;
	public bool m_Directional;
	public Vector3 m_ForceDirection;
	public float m_Radius;
	public Color m_Color = Color.white;
    public bool m_HasColor = true;
    public bool activated = false;
    BoxCollider2D box;
    float counter = 0;
    float cooldown = 0.05f;

    void Start ()
    {
        if (!VectorGrid.grid)
        {
            Debug.Log("No grid");
            Destroy(this);
        }

        m_VectorGrid = VectorGrid.grid;
        box = this.GetComponent<BoxCollider2D>();
    }

	// Update is called once per frame
	void Update () 
	{
		if(activated)
		{
            counter -= Time.deltaTime;

            if (counter <= 0)
            {
                if (m_Directional)
                {
                    m_VectorGrid.AddGridForce(this.transform.position, m_ForceDirection * m_ForceScale, m_Radius, m_Color, m_HasColor);
                }
                else
                {
                    m_VectorGrid.AddGridForce(this.transform.position, m_ForceScale, m_Radius, m_Color, m_HasColor);
                }
                counter = cooldown;
            }
		}
        // Check if over the grid
        /*
        else if(m_VectorGrid.mesh.bounds.Intersects(this.GetComponent<BoxCollider2D>().bounds))
        {
            activated = true;
        }*/
	}


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Grid")
        {
            activated = true;
        }
    }
}