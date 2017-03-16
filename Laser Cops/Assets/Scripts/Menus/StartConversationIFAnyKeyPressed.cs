using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartConversationIFAnyKeyPressed : MonoBehaviour 
{
    public ConversationManager convo_to_start;


	void Update () 
	{
        if (Input.anyKeyDown)
        {
            convo_to_start.Start_Conversation();
            Destroy(this.gameObject);
        }
    }
}
