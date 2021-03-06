using UnityEngine;
using System.Collections;

public class ScrollDemoManager : MonoBehaviour 
{
    public bool ignore_graphical_settings = false;
    public Vector2 m_ScrollSpeed;

	float m_ExplosiveForce = 1.0f;
	float m_ForceRadius = 1.0f;
	
	float m_Red = 0.0f;
	float m_Green = 0.0f;
	float m_Blue = 255.0f;
	bool m_RandomiseColor = true;
	
	public VectorGrid m_VectorGrid;
	
	Color m_StartColor = Color.red;
	Color m_TargetColor = Color.blue;
	float m_ColorInterp;
	
	void Start()
	{

    }
	
	// Update is called once per frame
	void Update () 
	{
		Color color = new Color(m_Red/255.0f, m_Green/255.0f, m_Blue/255.0f, 1.0f);
		
		if(m_RandomiseColor)
		{
			UpdateRandomColor();
		}

        if (ignore_graphical_settings || GraphicalSettings.graphical_settings.Scroll_Grid)  
		    m_VectorGrid.Scroll(m_ScrollSpeed * Time.deltaTime);
	}
	
	void UpdateRandomColor()
	{
		m_ColorInterp += Time.deltaTime;
		
		if(m_ColorInterp > 1.0f)
		{
			m_ColorInterp -= 1.0f;
			m_StartColor = m_TargetColor;
			m_TargetColor = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
		}
		
		Color interpolatedColor = m_StartColor + ((m_TargetColor - m_StartColor) * m_ColorInterp);
		m_Red = interpolatedColor.r * 255.0f;
		m_Green = interpolatedColor.g * 255.0f;
		m_Blue = interpolatedColor.b * 255.0f;

		m_VectorGrid.m_ThickLineSpawnColor = interpolatedColor;
	}
}
