using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarAnimation : MonoBehaviour
{
    public bool y_scale;
    float original_y;
    public bool x_scale;
    float original_x;
    public float expand_time = 1f;
    bool running = false;

    Image image;

    void Start()
    {
        image = this.GetComponent<Image>();

        StartPulsing(Color.green);
    }


    public void StartPulsing(Color desired_colour)
    {
        if (running)
            return;

        running = true;
        image.color = desired_colour;
    }


    void Update()
    {
        if (!running)
            return;

        // Start fading
        Color c = image.color;
        c.a -= Time.deltaTime * 1.5f;
        image.color = c;

        if (c.a <= 0)
        {
            running = false;
        }
    }
}
