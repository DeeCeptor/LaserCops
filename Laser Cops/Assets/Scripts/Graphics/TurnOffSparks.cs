using UnityEngine;
using System.Collections;

public class TurnOffSparks : MonoBehaviour
{
    public float time_remaining = 0.5f;
    public float start_time_remaining = 0.5f;
    ParticleSystem p;
    AudioSource audio;

    void Start ()
    {
        p = this.GetComponent<ParticleSystem>();
        audio = this.GetComponent<AudioSource>();
    }


    public void StartSparks()
    {
        time_remaining = start_time_remaining;

        if (!p.isPlaying)
        {
            p.Play();
            audio.Play();
        }
    }
    public void StopSparks()
    {
        if (p.isPlaying)
        {
            p.Stop();
            audio.Stop();
        }
        time_remaining = 0;
    }

    void Update()
    {
        time_remaining -= Time.deltaTime;

        if (time_remaining <= 0)
        {
            if (p.isPlaying)
            {
                p.Stop();
                audio.Stop();
            }
        }
        else
        {
            if (!p.isPlaying)
            {
                p.Play();
                audio.Play();
            }
        }
	}
}
