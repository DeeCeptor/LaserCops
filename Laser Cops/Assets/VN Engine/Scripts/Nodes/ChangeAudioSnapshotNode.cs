using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class ChangeAudioSnapshotNode : Node
{
    public AudioMixerSnapshot toAudioSnapshot;
    public float transitionTime;

    public override void Run_Node()
    {
        this.toAudioSnapshot.TransitionTo(this.transitionTime);

        Finish_Node();
    }


    public override void Finish_Node()
    {
        StopAllCoroutines();

        base.Finish_Node();
    }
}
