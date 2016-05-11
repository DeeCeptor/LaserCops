using UnityEngine;
using System.Collections;

public class DialogueNode : Node
{	
	public string actor;	// Name of actor to use for talking
	public string textbox_title;    // Name to be placed at top of textbox. Ex: 'Bob'
    private AudioSource voice_clip;   // Sound to play alongside the text. Ex: Voice saying the words in the text area
	[TextArea(3,10)]
	public string text;
    private string processed_text;
    public bool darken_all_other_characters = false;     // Darkens all other actors on the scene
    public bool bring_speaker_to_front = true;      // Changes the ordering so this actor will be in front of others
	public bool clear_text_after = false;

    [HideInInspector]
	public bool done_printing = false;
    [HideInInspector]
    public bool no_voice_playing = false;
    [HideInInspector]
    public bool done_voice_clip = false; // Set to true when the voice clip accompanying this dialogue is done playing
    private bool running = false;   // Set true when Run_Node is called
    
	public override void Run_Node()
	{
        running = true;

        UIManager.ui_manager.dialogue_text_panel.text = "";
		UIManager.ui_manager.speaker_text_panel.text = textbox_title;
		StartCoroutine(Animate_Text(text, VNProperties.delay_per_character));
        processed_text = ProcessEntireDialogue(text);

        // If the actor field is filled in and the actor is present on the scene
        Actor speaker = ActorManager.Get_Actor(actor);
        if (actor != "" 
            && speaker != null)
		{
			// Slightly darken all other actors
            if (darken_all_other_characters)
			    ActorManager.Darken_All_Actors_But(speaker);

			// Lighten this actor to show this one is talking
			speaker.Lighten();

            // Bring this actor to the front
            if (bring_speaker_to_front)
                ActorManager.Bring_Actor_To_Front(speaker);
		}

        if (voice_clip != null && voice_clip.clip != null)
        {
            // Play audio if there is any
            //AudioManager.audio_manager.Play_Voice_Clip(voice_clip);
            voice_clip.volume = AudioManager.audio_manager.voice_volume;
            voice_clip.Play();
        }
        else
        {
            done_voice_clip = true;
            no_voice_playing = true;
        }
	}

	public override void Button_Pressed()
	{
		if (done_printing)
			Finish_Node();
		else
		{
			// Show all the text in this dialogue. This lets fast readers click to show all the dialog
			// instead of waiting it all to appear. User must click again to finish this dialogue piece.
			done_printing = true;
			StopAllCoroutines();
			UIManager.ui_manager.dialogue_text_panel.text = processed_text;
		}
	}
	
	public override void Finish_Node()
	{
        if (voice_clip != null)
            voice_clip.Stop();
        /*
        if(AudioListener.volume == 0)
        {
            voice_clip.Stop();
        }*/

		StopAllCoroutines();

		if (clear_text_after)
		{
			UIManager.ui_manager.speaker_text_panel.text = "";
			UIManager.ui_manager.dialogue_text_panel.text = "";
		}

		// Record what was said in the log so players can go back and read anything they missed
		SceneManager.scene_manager.Add_To_Log(textbox_title, processed_text + "\n");
        done_printing = false;
        done_voice_clip = false;
        running = false;
        base.Finish_Node();
	}


	// Prints the text to the UI manager's dialogue text one character at a time.
	// It waits time_between_characters seconds before adding on the next character.
	public IEnumerator Animate_Text(string strComplete, float time_between_characters) 
	{
		int i = 0;
        bool italics = false;
		while(i < strComplete.Length)
		{
            if (!UIManager.ui_manager.entire_UI_panel.activeInHierarchy)
            {
                done_printing = true;
            }

            bool ignore = false;
            char next_char = (char) strComplete[i++];   // Next character to be printed

            // Check for italics
            switch (next_char)
            {
                case '`':
                    ignore = true; //make sure this character isn't printed by ignoring it
                    italics = !italics; //toggle italic styling
                    break;
            }

            if (ignore)
                continue;
            if (!italics)    // Regular text
                UIManager.ui_manager.dialogue_text_panel.text += next_char;
            else if (italics)    // Italics text
                UIManager.ui_manager.dialogue_text_panel.text += "<i>" + next_char + "</i>";

            if (SceneManager.text_scroll_speed != 0)
			    yield return new WaitForSeconds(SceneManager.text_scroll_speed);
		}
		done_printing = true;
	}


    string ProcessEntireDialogue(string dialogue)
    {
        string processed_dialogue = "";
        int i = 0;
        bool italics = false;
        while (i < dialogue.Length)
        {
            if (!UIManager.ui_manager.entire_UI_panel.activeInHierarchy)
            {
                done_printing = true;
            }

            bool ignore = false;
            char next_char = (char)dialogue[i++];   // Next character to be printed

            // Check for italics
            switch (next_char)
            {
                case '`':
                    ignore = true; //make sure this character isn't printed by ignoring it
                    italics = !italics; //toggle italic styling
                    break;
            }

            if (ignore)
                continue;
            if (!italics)    // Regular text
                processed_dialogue += next_char;
            else if (italics)    // Italics text
                processed_dialogue += "<i>" + next_char + "</i>";
        }
        return processed_dialogue;
    }


	void Start ()
    {
        voice_clip = gameObject.GetComponent<AudioSource>();//.clip;
    }
	

	void Update ()
    {
        // If running and playing voice, keep checking to see if we're done our voice clip
        if (running
            && !done_voice_clip)
        {
            voice_clip.volume = AudioManager.audio_manager.voice_volume;    // Constantly set the volume to the correct level
            done_voice_clip = !voice_clip.isPlaying;
            //!AudioManager.audio_manager.voice_audio_source.isPlaying;
        }
	}
}
