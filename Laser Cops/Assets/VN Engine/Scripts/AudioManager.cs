using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audio_manager;
    public AudioSource background_music_audio_source;    // Music that plays in background. Only one track of music will ever be playing at once
    public AudioSource voice_audio_source;    // Audio source playing the talking of the voice actors

    bool muted = false;     // If muted, NO audio will play
    public GameObject sound_effect_prefab;  // Must be an object with an audiosource, and necessary for PlaySoundEffect nodes

    // Volume controls
    [HideInInspector]
    public float master_volume = 1;
    [HideInInspector]
    public float music_volume = 1;
    [HideInInspector]
    public float voice_volume = 1;
    [HideInInspector]
    public float effects_volume = 1;


    void Awake ()
    {
        audio_manager = this;
    }
	void Start ()
    {
	
	}


    // Fades out the current background music over seconds_it_takes_to_fade_out
    public IEnumerator Fade_Out_Music(float seconds_it_takes_to_fade_out)
    {
        
        if (background_music_audio_source != null)
        {
            while (background_music_audio_source.volume > 0)
            {
                background_music_audio_source.volume -= 0.1f;
                yield return new WaitForSeconds(seconds_it_takes_to_fade_out / 100.0f);
            }
        }
    }

    // Starts the new background music and starts it playing
    public void Set_Music(AudioClip new_music)
    {
        background_music_audio_source.clip = new_music;
        background_music_audio_source.volume = 1;
        background_music_audio_source.loop = true;
        background_music_audio_source.Play();
    }


    // Toggles the muting of ALL audio in the scene
    public void Toggle_Audio_Muting()
    {
        Debug.Log("Toggling audio muting");

        if (!muted)
            AudioListener.volume = 0;
        else
            AudioListener.volume = master_volume;

        muted = !muted;
    }


    public void Play_Voice_Clip(AudioClip voice_clip)
    {
        voice_audio_source.Stop();
        voice_audio_source.clip = voice_clip;
        voice_audio_source.Play();
    }
    

    // Creates a new gameobject with its own AudioSource that destroys itself after the sound is done playing
    public void Play_Sound_Effect(AudioClip voice_clip)
    {
        GameObject sound_obj = Instantiate(sound_effect_prefab) as GameObject;
        sound_obj.GetComponent<AudioSource>().clip = voice_clip;
        sound_obj.GetComponent<AudioSource>().playOnAwake = true;
    }


    // Volume options managed by the sliders in the pause menu
    public void Master_Volume_Changed(float new_volume)
    {
        master_volume = new_volume;

        // Change the audio listener's volume
        AudioListener.volume = master_volume;
    }
    public void Voice_Volume_Changed(float new_volume)
    {
        voice_volume = new_volume;
        voice_audio_source.volume = voice_volume;
    }
    public void Music_Volume_Changed(float new_volume)
    {
        music_volume = new_volume;
        background_music_audio_source.volume = music_volume;
    }
    public void Effects_Volume_Changed(float new_volume)
    {
        effects_volume = new_volume;
    }
}
