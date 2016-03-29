using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour
{
    public float time_left = 2.0f; // Time remaining before we turn off blinkers
    public float light_duration = 0.4f;
    public bool start_active = false;

    float prev_interval;
    bool on = false;
    SpriteRenderer sprite;

    void Awake ()
    {
        sprite = this.GetComponent<SpriteRenderer>();
    }
    void Start ()
    {
        if (start_active)
        {
            StartBlinking(time_left, light_duration);
        }
        else
        {
            StopBlinking();
        }
    }


    public void StartBlinking(float duration, float light_intervals)
    {
        time_left = duration;
        on = true;
        sprite.enabled = true;
        prev_interval = light_intervals;
        light_duration = light_intervals;
    }
    public void StopBlinking()
    {
        on = false;
        time_left = 0;
        sprite.enabled = false;
    }

    void Update ()
    {
	    if (time_left > 0)
        {
            time_left -= Time.deltaTime;
            prev_interval -= Time.deltaTime;

            // Toggle light to create blinking effect
            if (prev_interval <= 0)
            {
                sprite.enabled = !sprite.enabled;
                prev_interval = light_duration;
            }
        }
        else if (on)
        {
            StopBlinking();
        }
    }
}
