using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExpandUIImageToScale : MonoBehaviour
{
    public bool y_scale;
    float original_y;
    public bool x_scale;
    float original_x;
    public float expand_time = 0.5f;
    public bool fade_sprite_after = false;
    public bool currently_expanding = false;
    public bool start_expanding = true;
    Image sprite;

    void Start()
    {
        //sprite = this.GetComponent<Image>();

        if (start_expanding)
            StartExpanding();
    }


    public void StartExpanding()
    {
        if (currently_expanding)
            return;

        if (y_scale)
        {
            original_y = this.transform.localScale.y;
            this.transform.localScale = new Vector3(this.transform.localScale.x, 0, this.transform.localScale.z);
            currently_expanding = true;
            //ResetColor();
        }
        if (x_scale)
        {
            original_x = this.transform.localScale.x;
            this.transform.localScale = new Vector3(0, this.transform.localScale.y, this.transform.localScale.z);
            currently_expanding = true;
            //ResetColor();
        }
    }
    void ResetColor()
    {
        Color c = sprite.color;
        c.a = 1f;
        sprite.color = c;
    }


    void Update()
    {
        if (currently_expanding)
        {
            if (y_scale && this.transform.localScale.y != original_y ||
                x_scale && this.transform.localScale.x != original_x)
            {
                if (y_scale && this.transform.localScale.y != original_y)
                    this.transform.localScale = new Vector3(this.transform.localScale.x,
                        Mathf.Min(this.transform.localScale.y + Time.deltaTime * expand_time * original_y, original_y),
                        this.transform.localScale.z);
                if (x_scale && this.transform.localScale.x != original_x)
                    this.transform.localScale = new Vector3(Mathf.Min(this.transform.localScale.x + Time.deltaTime * expand_time * original_x, original_x),
                        this.transform.localScale.y,
                        this.transform.localScale.z);
            }
            else if (fade_sprite_after && sprite.color.a > 0)
            {
                Color c = sprite.color;
                c.a -= Time.deltaTime * expand_time;
                sprite.color = c;
            }
            else
                currently_expanding = false;
        }
    }
}
