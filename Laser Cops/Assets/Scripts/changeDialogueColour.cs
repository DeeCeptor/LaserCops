using UnityEngine;
using System.Collections;

public class changeDialogueColour : MonoBehaviour {
    //Colour to switch dialogue to
    public Color colorDialogueToBecome;
    //placeholder for colour that is being replaced
    private Color switchBackDialogueColor;
    //Colour to switch speaker name to
    public Color colorSpeakerToBecome;
    private Color switchBackSpeakerColor;
    //how long to wait before changing color
    public float waitTime = 9f;
    public float waitTimer;
    //how long to wait before changing back
    public float backTime = 5f;
    public float backTimer;
    public bool changed = false;

	// Use this for initialization
	void Start () {
        waitTimer = Time.time + waitTime;
	}

    void OnEnable()
    {
        waitTimer = Time.time + waitTime;
    }
	
	void Update ()
    {
        /*
	if(waitTimer < Time.time & !changed)
        {
            switchBackDialogueColor = UIManager.ui_manager.dialogue_text_panel.color;
            switchBackSpeakerColor = UIManager.ui_manager.speaker_text_panel.color;
            UIManager.ui_manager.dialogue_text_panel.color = colorDialogueToBecome;
            UIManager.ui_manager.speaker_text_panel.color = colorSpeakerToBecome;
            changed = true;
            backTimer = Time.time + backTime;
        }
    else if(changed & backTimer < Time.time)
        {
            UIManager.ui_manager.dialogue_text_panel.color = switchBackDialogueColor;
            UIManager.ui_manager.speaker_text_panel.color = switchBackSpeakerColor;
        }*/
	}
}
