using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SceneManager : MonoBehaviour 
{
	public ConversationManager starting_conversation;

	[HideInInspector]
	public static ConversationManager current_conversation;
	[HideInInspector]
	public static SceneManager scene_manager;
	[HideInInspector]
    public bool fast_forwarding = false;

    private string conversation_log;    // Log of all previous conversations
    public string Conversation_log
    {
        get
        {
            return conversation_log;
        }

        set
        {
            conversation_log = value;
            UIManager.ui_manager.log_text.text = conversation_log;
        }
    }

    private float fast_forward_delay = 0.3f;     // When holding down the fast forward button or the SPACE bar, how fast should button presses be sent?

    [HideInInspector]
    public static float text_scroll_speed = 0.02f;     // How fast characters are displayed from dialogues


	void Start () 
	{
		scene_manager = this;

		// Start the first conversation
		StartCoroutine(Start_Scene());
	}

	public IEnumerator Start_Scene()
	{
		yield return new WaitForSeconds(0.2f);
        try
        {
            starting_conversation.Start_Conversation();
        }
        catch (Exception e)
        {
            Debug.LogError("No starting Conversation set. Please drag in a Conversation into the SceneManager's Starting Conversation field.\n" + e.Message);
        }
    }


	// Pass in the game object that contains a 'ConversationManager' script to start
	public void Start_Conversation(GameObject conversation)
	{
		conversation.GetComponent<ConversationManager>().Start_Conversation();
	}


	public void Add_To_Log(string heading, string text)
	{
        if (!string.IsNullOrEmpty(heading))
            Conversation_log += heading + ": ";
        Conversation_log += text + "\n";
	}


    // Calls Button_Pressed on the current conversation
    // Hierarchy of button presses: SceneManager -> Current Conversation -> Current Node
    public void Button_Pressed()
    {
        current_conversation.Button_Pressed();
    }

    float super_speed_delay = 0;


    void Update () 
	{
        super_speed_delay -= Time.deltaTime * Time.timeScale;

        // Check for user input
        if (Input.GetKeyDown(KeyCode.Return)    // Enter
            || Input.GetKeyDown(KeyCode.KeypadEnter)    // Keypad enter
            || (super_speed_delay <= 0 && (Input.GetKey(KeyCode.Space) ))   // Holding down space bar
            || (fast_forwarding && super_speed_delay <= 0)  // Holding down the 'FAST' button
            )
        {
            super_speed_delay = fast_forward_delay;
            this.Button_Pressed();
        }
    }

    // Sends a button press every fast_forward_delay seconds
    public void Fast_Forward()
    {
        fast_forwarding = true;
        /*
        if (super_speed_delay <= 0)
        {
            super_speed_delay = fast_forward_delay;
            this.Button_Pressed();
        }*/
    }
    public void Stop_Fast_Forward()
    {
        fast_forwarding = false;
    }

    public void Text_Scroll_Speed_Change(float new_speed)
    {
        text_scroll_speed = new_speed;
    }
}
