using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialTextToBox : MonoBehaviour
{
    public bool active = false;
    public ConversationManager conversation_to_start;


	void Start ()
    {
	
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("MainCamera"))
        {
            Activate();
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (!active)
        {
            if (collider.gameObject.tag.Equals("MainCamera"))
            {
                Activate();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "MainCamera")
        {
            Destroy(gameObject);
        }
    }

    public void Activate()
    {
        VNSceneManager.scene_manager.Start_Conversation(conversation_to_start.gameObject);
        active = true;
        Destroy(this.gameObject);
    }
}
