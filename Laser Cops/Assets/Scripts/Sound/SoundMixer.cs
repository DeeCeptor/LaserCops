using UnityEngine;
using System.Collections.Generic;
using DigitalRuby.SoundManagerNamespace;
using UnityEngine.UI;

public class SoundMixer : MonoBehaviour
{
    public static SoundMixer sound_manager;
    
    public static float music_volume_scale = 1.0f;
    public static float sound_effect_scale = 1.0f;

    // SOUND EFFECTS
    public List<AudioSource> Sounds = new List<AudioSource>();

    // MUSIC
    public List<AudioSource> Music = new List<AudioSource>();
    public AudioSource cur_music;   // Track currently playing
    public AudioSource starting_music;

    public bool starting_music_persists = false;

    float music_volume_modifier = 0.7f;

    void Awake ()
    {
        sound_manager = this;
    }
    void Start ()
    {
        if (starting_music)
            PlayMusic(starting_music);
    }


    void Update ()
    {
	
	}


    public void MusicVolumeChanged(float new_value)
    {
        //music_volume_scale = new_value;
        SoundManager.MusicVolume = new_value;
        /*
        music_volume_modifier = new_value;
        if (starting_music != null)
        {
            starting_music.volume = new_value;
        }
        if (cur_music != null)
            cur_music.volume = new_value;*/
    }
    public void EffectsVolumeChanged(float new_value)
    {
        //sound_effect_scale = new_value;
        SoundManager.SoundVolume = new_value;
    }


    public void PlaySound(AudioSource sound, float volume_scale)
    {
        sound.PlayOneShotSoundManaged(sound.clip, volume_scale);
    }
    public void StopSound(AudioSource sound)
    {
        sound.Stop();
    }
    public void PlayMusic(AudioSource music)
    {
        cur_music = music;

        // Display the track name & artist
        if (FadeTextInAndOut.music_info != null)
        {
            FadeTextInAndOut.music_info.GetComponent<Text>().text = "♪ " + music.clip.name;
            FadeTextInAndOut.music_info.Start_Fading_In_Then_Out();
            FadeTextInAndOut.music_info.gameObject.SetActive(true);
        }
        SoundManager.StopAllLoopingSounds();
        music.PlayLoopingMusicManaged(music_volume_modifier * music_volume_scale, 1.0f, false);
    }
    public void StopMusic()
    {
        cur_music.StopLoopingMusicManaged();
    }


    public void VictoryFanfare()
    {
        GameObject obj = (GameObject) GameObject.Instantiate(Resources.Load("Success sound") as GameObject);
        SoundManager.StopAllLoopingSounds();
        obj.GetComponent<AudioSource>().PlayOneShotMusicManaged(obj.GetComponent<AudioSource>().clip);
    }
    public void DefeatFanfare()
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Defeat sound") as GameObject);
        SoundManager.StopAllLoopingSounds();
        obj.GetComponent<AudioSource>().PlayOneShotMusicManaged(obj.GetComponent<AudioSource>().clip);
    }
    public void StopAllSound()
    {
        SoundManager.StopAll();
    }


    // SOUND EFFECTS
    public void PlayLazerShot()
    {
        PlaySound(Sounds[0], 1f * sound_effect_scale);
    }
    public void PlayNotification()
    {
        PlaySound(Sounds[1], 1f * sound_effect_scale);
    }
    public void PlayShortSpark()
    {
        PlaySound(Sounds[2], 1f * sound_effect_scale);
    }
    public void PlayLongElectricShock()
    {
        PlaySound(Sounds[3], 1f * sound_effect_scale);
    }
    public void PlayCarRev()
    {
        PlaySound(Sounds[4], 0.3f * sound_effect_scale);
    }
    public void PlayLong8bitLazer()
    {
        PlaySound(Sounds[5], 1f * sound_effect_scale);
    }
    public void Play8bitExplosion()
    {
        PlaySound(Sounds[6], 1f * sound_effect_scale);
    }
    public void PlayChargeUp()
    {
        PlaySound(Sounds[7], 0.5f * sound_effect_scale);
    }
    public void StopChargeUp()
    {
        StopSound(Sounds[7]);
    }
    public void PlayGrinding()
    {
        PlaySound(Sounds[8], 0.2f * sound_effect_scale);
    }
    public void PlaySyncopatedLazer()
    {
        PlaySound(Sounds[9], 1f * sound_effect_scale);
    }
    public void PlayTransferHealth()
    {
        PlaySound(Sounds[10], 1f * sound_effect_scale);
    }
    public void Play8bitBeep()
    {
        PlaySound(Sounds[11], 1f * sound_effect_scale);
    }
    public void PlayObstacleWarning()
    {
        PlaySound(Sounds[12], 0.4f * sound_effect_scale);
    }
    public void PlayGettingHitExplosion()
    {
        PlaySound(Sounds[13], 0.5f * sound_effect_scale);
    }
    public void PlayGettingHit()
    {
        PlaySound(Sounds[14], 0.5f * sound_effect_scale);
    }
    public void PlayBigLazerSound()
    {
        PlaySound(Sounds[15], 0.3f * sound_effect_scale);
    }
    public void StopBigLazerSound()
    {
        StopSound(Sounds[15]);
    }
    public void PlayCollectSound()
    {
        PlaySound(Sounds[16], 1.5f * sound_effect_scale);
    }

    // MUSIC
    /*
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

    // Original music
    public void PlayZoombaTitle()
    {
        PlayMusic(Music[5]);
    }
    public void PlayInPursuit()
    {
        PlayMusic(Music[6]);
    }
    public void PlayLZMainTheme()
    {
        PlayMusic(Music[7]);
    }*/
}
