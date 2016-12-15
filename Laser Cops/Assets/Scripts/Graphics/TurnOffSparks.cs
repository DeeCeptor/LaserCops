using UnityEngine;
using System.Collections;

public class TurnOffSparks : MonoBehaviour
{
    public float time_remaining = 0.9f;
    public float start_time_remaining = 0.5f;
    ParticleSystem p;
    AudioSource _audio;

    void Start ()
    {
        p = this.GetComponent<ParticleSystem>();
        _audio = this.GetComponent<AudioSource>();
    }


    public void StartSparks()
    {
        time_remaining = start_time_remaining;
        PlaySparks();
    }
    public void PlaySparks()
    {
        //this.gameObject.SetActive(true);
        p.enableEmission = true;
        if (!_audio.isPlaying)
        {
            _audio.Play();
        }
    }
    public void StopSparks()
    {
        p.enableEmission = false;
        if (_audio.isPlaying)
        {
            //p.Stop();
            _audio.Stop();
        }
        //this.gameObject.SetActive(false);
        time_remaining = 0;
    }

    void Update()
    {
        time_remaining -= Time.deltaTime;
        _audio.volume = AudioManager.audio_manager.effects_volume * 0.4f;

        if (time_remaining <= 0)
        {
            StopSparks();
            //this.gameObject.SetActive(false);
        }
        else
        {
            PlaySparks();
        }
    }
}
