using UnityEngine;
using System.Collections;

public class TurnOffSparks : MonoBehaviour
{
    public float time_remaining = 0.5f;
    public float start_time_remaining = 0.5f;
    ParticleSystem p;

    void Start ()
    {
        p = this.GetComponent<ParticleSystem>();
    }


    void Update()
    {
        time_remaining -= Time.deltaTime;

        if (time_remaining <= 0)
        {
            if (p.isPlaying)
               p.Stop();
        }
        else
        {
            if (!p.isPlaying)
               p.Play();
        }
	}
}
