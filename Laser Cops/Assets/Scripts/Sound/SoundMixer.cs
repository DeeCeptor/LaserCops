using UnityEngine;
using System.Collections.Generic;
using DigitalRuby.SoundManagerNamespace;

public class SoundMixer : MonoBehaviour
{
    public static SoundMixer sound_manager;

    // SOUND EFFECTS
    public List<AudioSource> Sounds = new List<AudioSource>();

    // MUSIC
    public List<AudioSource> Music = new List<AudioSource>();
    public AudioSource cur_music;   // Track currently playing


    void Awake ()
    {
        sound_manager = this;
    }
    void Start ()
    {
        PlayRaiser();
    }


    void Update ()
    {
	
	}


    public void PlaySound(AudioSource sound, float volume_scale)
    {
        sound.PlayOneShotSoundManaged(sound.clip, volume_scale);
    }
    public void PlayMusic(AudioSource music)
    {
        cur_music = music;
        music.PlayLoopingMusicManaged(1.0f, 1.0f, true);
    }
    public void StopMusic()
    {
        cur_music.StopLoopingMusicManaged();
    }


    // SOUND EFFECTS
    public void PlayLazerShot()
    {
        PlaySound(Sounds[0], 1f);
    }
    public void PlayNotification()
    {
        PlaySound(Sounds[1], 1f);
    }
    public void PlayShortSpark()
    {
        PlaySound(Sounds[2], 1f);
    }
    public void PlayLongElectricShock()
    {
        PlaySound(Sounds[3], 1f);
    }
    public void PlayCarRev()
    {
        PlaySound(Sounds[4], 0.3f);
    }
    public void PlayLong8bitLazer()
    {
        PlaySound(Sounds[5], 1f);
    }
    public void Play8bitExplosion()
    {
        PlaySound(Sounds[6], 1f);
    }
    public void PlayChargeUp()
    {
        PlaySound(Sounds[7], 1f);
    }
    public void PlayGrinding()
    {
        PlaySound(Sounds[8], 1f);
    }
    public void PlaySyncopatedLazer()
    {
        PlaySound(Sounds[9], 1f);
    }


    // MUSIC
    public void Play2ndBallad()
    {
        PlayMusic(Music[0]);
    }
    public void PlayCvmLoop()
    {
        PlayMusic(Music[1]);
    }
    public void PlayCyberium()
    {
        PlayMusic(Music[2]);
    }
    public void PlayNoSleep()
    {
        PlayMusic(Music[3]);
    }
    public void PlayRaiser()
    {
        PlayMusic(Music[4]);
    }
}