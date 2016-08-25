using UnityEngine;
using System.Collections;

public class ColourPingPong : MonoBehaviour 
{
    public Color colour_1;
    public Color colour_2;

    public float z_speed = -5;

	void Start () 
	{
	
	}
	

	void Update () 
	{
        this.GetComponent<TextMesh>().color = Color.Lerp(colour_1, colour_2, Mathf.PingPong(Time.time, 1f));

        // Zoom in
        this.transform.position = new Vector3(this.transform.position.x, 
                                              this.transform.position.y, 
                                              this.transform.position.z + z_speed * Time.deltaTime);
    }
}
