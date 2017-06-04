using UnityEngine;
using System.Collections;

public class VectorGridForce : MonoBehaviour 
{
    [HideInInspector]
	public VectorGrid m_VectorGrid;
    public float m_ForceScale = 2f;
    public float m_Radius = 1.5f;
    public bool m_Directional;
	public Vector3 m_ForceDirection;
	public Color m_Color = Color.white;
    public bool m_HasColor = true;
    public bool activated = false;
    BoxCollider2D box;
    float counter = 0;
    float wake_cooldown = 0.08f;
    public bool ignore_graphics_settings = false;

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
                else if (ignore_graphics_settings)
                {
                    m_VectorGrid.AddGridForce(this.transform.position, m_ForceScale, m_Radius, m_Color, m_HasColor);
                }
                else
                {
                    EffectsManager.effects.GridWake(this.transform.position, m_ForceScale, m_Radius, m_Color, true);
                }
                counter = wake_cooldown;
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
