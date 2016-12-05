using UnityEngine;
using System.Collections;

public class ExpandToOriginalScale : MonoBehaviour 
{
    public bool y_scale;
    float original_y;
    public bool x_scale;
    float original_x;
    public float expand_time = 0.5f;
    public bool fade_sprite_after = false;

	void Start () 
	{
	    if (y_scale)
        {
            original_y = this.transform.localScale.y;
            this.transform.localScale = new Vector3(this.transform.localScale.x, 0, this.transform.localScale.z);
        }
        if (x_scale)
        {
            original_x = this.transform.localScale.x;
            this.transform.localScale = new Vector3(0, this.transform.localScale.y, this.transform.localScale.z);
        }
    }
	

	void Update () 
	{
        if (y_scale && this.transform.localScale.y != original_y)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x,
                Mathf.Min(this.transform.localScale.y + Time.deltaTime * expand_time * original_y, original_y),
                this.transform.localScale.z);
        }
        else if (fade_sprite_after)
        {
            Color c = this.GetComponent<SpriteRenderer>().color;
            c.a -= Time.deltaTime;
            this.GetComponent<SpriteRenderer>().color = c;
        }
        if (x_scale && this.transform.localScale.x != original_x)
        {
            this.transform.localScale = new Vector3(Mathf.Min(this.transform.localScale.x + Time.deltaTime * expand_time * original_x, original_x),
                this.transform.localScale.y,
                this.transform.localScale.z);
        }
    }
}
