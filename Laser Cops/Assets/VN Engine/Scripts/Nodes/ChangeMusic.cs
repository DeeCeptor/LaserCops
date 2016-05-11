using UnityEngine;
using System.Collections;

public class ChangeMusic : Node
{

    public override void Run_Node()
    {
        
        StartCoroutine(AudioManager.audio_manager.Fade_Out_Music(5));
        AudioManager.audio_manager.Set_Music(gameObject.GetComponent<AudioSource>().clip);
        Finish_Node();
    }


    public override void Finish_Node()
    {
        StopAllCoroutines();

        base.Finish_Node();
    }
}
