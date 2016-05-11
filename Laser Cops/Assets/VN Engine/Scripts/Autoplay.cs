using UnityEngine;
using System.Collections;

// If enabled, the story automatically progresses, sending 'button presses' after all the dialogue in a node has been printed out
public class Autoplay : MonoBehaviour
{
    bool auto_playing = false;
    bool button_to_be_pressed = false;
    float auto_play_delay = 1.5f;   // How long we wait after the voice and text is done playing
    bool isAudioMuted;
    DialogueNode cur_dialogue;


	void Start ()
    {
	
	}


    public void ToggleAutoPlay()
    {
        auto_playing = !auto_playing;
        Debug.Log("Auto play set: " + auto_playing);
    }

	
	void Update ()
    {
        if (auto_playing
            && SceneManager.current_conversation != null)
        {
            // Check if our current node is a dialogue node
            Node cur_node = SceneManager.current_conversation.Get_Current_Node();
            if (cur_node is DialogueNode)
            {
                DialogueNode dialogue = (DialogueNode)cur_node;

                if(AudioListener.volume == 0)
                {
                    isAudioMuted = true;
                    auto_play_delay = 1.5f;
                }
                else
                {
                    isAudioMuted = false;
                    auto_play_delay = 1.0f;
                }

                if ((dialogue.done_printing)// || !UIManager.ui_manager.entire_UI_panel.activeSelf) // Done printing, or UI panel is hidden
                    && (dialogue.done_voice_clip || isAudioMuted)
                    && !button_to_be_pressed)
                {
                    cur_dialogue = dialogue;
                    button_to_be_pressed = true;
                    Debug.Log("Autoplaying");
                    // Done printing and voice, wait a second or two then press the button
                    StartCoroutine(Delay_Button_Press());
                }
            }
        }
	}


    IEnumerator Delay_Button_Press()
    {
        yield return new WaitForSeconds(auto_play_delay);

        Node cur_node = SceneManager.current_conversation.Get_Current_Node();
        if (cur_node is DialogueNode)
        {
            DialogueNode dialogue = (DialogueNode)cur_node;

            if (dialogue == cur_dialogue)
            {
                button_to_be_pressed = false;
                SceneManager.current_conversation.Button_Pressed();
            }
            else
            {
                button_to_be_pressed = false;
            }
        }
        else
            button_to_be_pressed = false;
    }
}
