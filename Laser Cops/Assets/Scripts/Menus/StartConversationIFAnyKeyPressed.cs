using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartConversationIFAnyKeyPressed : MonoBehaviour 
{
    public ConversationManager convo_to_start;


	void Update () 
	{
        if (Input.anyKeyDown
            || (InputManager.ActiveDevice != null && InputManager.ActiveDevice.AnyButtonWasPressed))
        {
            convo_to_start.Start_Conversation();
            Destroy(this.gameObject);
        }
    }
}
