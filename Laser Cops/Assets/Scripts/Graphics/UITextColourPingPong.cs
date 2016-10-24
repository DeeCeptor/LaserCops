using UnityEngine;
using UnityEngine.UI;

public class UITextColourPingPong : MonoBehaviour 
{
    public Color colour_1, colour2;
    public float speed = 1f;
    Text t;

	void Start () 
	{
        t = this.GetComponent<Text>();
	}
	

	void Update () 
	{
        t.color = Color.Lerp(colour_1, colour2, Mathf.PingPong(Time.time, speed));
    }
}
