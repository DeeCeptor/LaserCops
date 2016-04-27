using UnityEngine;
using System.Collections;

public class TurnOffSparks : MonoBehaviour
{
    public float time_remaining = 0.9f;
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

        //this.gameObject.SetActive(true);

        p.enableEmission = true;
        if (!audio.isPlaying)
        {
            audio.Play();
        }
    }
    public void StopSparks()
    {
        p.enableEmission = false;
        if (audio.isPlaying)
        {
            //p.Stop();
            audio.Stop();
        }
        //this.gameObject.SetActive(false);
        time_remaining = 0;
    }

    void Update()
    {
        time_remaining -= Time.deltaTime;

        if (time_remaining <= 0)
        {
            StopSparks();
            //this.gameObject.SetActive(false);
        }
        else
        {
            p.enableEmission = true;
            if (!audio.isPlaying)
            {
                audio.Play();
            }
        }
    }
}
