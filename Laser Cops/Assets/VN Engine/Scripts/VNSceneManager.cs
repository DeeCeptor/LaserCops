using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

public class VNSceneManager : MonoBehaviour 
{
	public ConversationManager starting_conversation;

	[HideInInspector]
	public static ConversationManager current_conversation;
	[HideInInspector]
	public static VNSceneManager scene_manager;
	[HideInInspector]
    public bool fast_forwarding = false;

    // LOCALIZATION
    public TextAsset Localized_Dialogue_CSV;    // Assign a CSV to this field if you wish to use use localization (support for multiple languages)
    [HideInInspector]
    public Dictionary<string, Dictionary<string, string>> Localized_Dialogue_Dictionaries;    // One dictionary is assigned per language
    // LOCALIZATION

    public bool hide_UI_at_start = false;   // If true, the UI panel is hidden. IT can be show with either a Show/Hide UI Node, or automatically when a Dialogue Node is run

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
        }
    }

    public static bool Waiting_till_true = false;   // Used by WaitNodes. Game will pause until this is set to true. Can be set by code: VNSceneManager.Waiting_till_true = true;

    private float fast_forward_delay = 0.3f;     // When holding down the fast forward button or the SPACE bar, how fast should button presses be sent?

    // Time played, in seconds
    public float play_time;


    // DEFAULT VALUES.  Changes are saved when edited in-game using PlayerPrefs
    // The delay between printing the next text character. The lower, the faster the speed.
    [HideInInspector]
    public static float text_scroll_speed = 0.04f;     // How fast characters are displayed from dialogues
    // How large the font of the text is
    [HideInInspector]
    public static int text_size = 25;
    // END DEFAULT VALUES


    void Awake ()
    {
        scene_manager = this;
        Time.timeScale = 1;

        if (Localized_Dialogue_CSV != null)
            Localized_Dialogue_Dictionaries = CSVReader.Generate_Localized_Dictionary(Localized_Dialogue_CSV);
        else
            Debug.Log("No Localized_Dialogue_CSV specified. Localization for DialogueNodes is not available", this.gameObject);
    }

    void Start () 
	{
        // Hide UI initially?
        UIManager.ui_manager.entire_UI_panel.SetActive(!hide_UI_at_start);

        
        // Load text scrolling speed
        //Text_Scroll_Speed_Change(PlayerPrefs.GetFloat("TextScrollSpeed", 0.02f));

        // Load the size of the dialogue text 
        //Text_Size_Change(PlayerPrefs.GetInt("TextSize", 25));

        // Set the best fit auto size
        //Text_Autosize_Change(System.Convert.ToBoolean(PlayerPrefs.GetInt("TextAutosize", 1)));

        // Enable and load multiple fonts
        //UIManager.ui_manager.font_drop_down.EnableFontMenu();

        // Start the first conversation
        StartCoroutine(Start_Scene());

        //StatsManager.Load_All_Items();
	}

	public IEnumerator Start_Scene()
	{
		yield return new WaitForSeconds(0.2f);
        try
        {
            if (starting_conversation != null)
                starting_conversation.Start_Conversation();
        }
        catch (Exception e)
        {
            Debug.Log("No starting Conversation set. Drag in a Conversation into the SceneManager's Starting Conversation field.\n" + e.Message, gameObject);
        }
    }


	// Pass in the game object that contains a 'ConversationManager' script to start
	public void Start_Conversation(GameObject conversation)
	{
		conversation.GetComponent<ConversationManager>().Start_Conversation();
	}
    public void Start_Conversation(ConversationManager conversation)
    {
        conversation.Start_Conversation();
    }


	public void Add_To_Log(string heading, string text)
	{
        /*
        Conversation_log += "\n";
        if (!string.IsNullOrEmpty(heading))
            Conversation_log += "<b>" + heading + "</b>: ";
        Conversation_log += text;
        UIManager.ui_manager.scroll_log_rect.verticalNormalizedPosition = 0;*/
	}

    /// </summary>
    // Calls Button_Pressed on the current conversation
    // Hierarchy of button presses: SceneManager -> Current Conversation -> Current Node
    /// </summary>
    public void Button_Pressed()
    {
        current_conversation.Button_Pressed();
    }


    // Goes to the previous node in the Conversation. Cannot go to a previous Conversation. 
    // If allowing this feature, be careful with how many Conversations you use, as you cannot go back to a previous Conversation
    public void Back_Button_Pressed()
    {
        if (current_conversation)
        {
            current_conversation.Go_Back_One_Node();
        }
    }


    float super_speed_delay = 0;


    void Update () 
	{
        play_time += Time.deltaTime;    // Does not increase when paused

        super_speed_delay -= Time.deltaTime * Time.timeScale;

        // Check for user input

        // If the user is holding down any of the below buttons, make it go SUPER FAST
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
    }
    public void Stop_Fast_Forward()
    {
        fast_forwarding = false;
    }


    // Returns the associated value with the given key for the language that has been set
    // Returns "" if the key is null or empty
    public string Get_Localized_Dialogue_Entry(string key)
    {
        // Check for any potential errors
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError("Get_Localized_Dialogue_Entry key passed in is null or empty", this.gameObject);
            return "";
        }
        if (!Localized_Dialogue_Dictionaries.ContainsKey(LocalizationManager.Language))
        {
            Debug.LogError("Get_Localized_Dialogue_Entry could not find language " + LocalizationManager.Language, this.gameObject);
            return "";
        }
        if (!Localized_Dialogue_Dictionaries[LocalizationManager.Language].ContainsKey(key))
        {
            Debug.LogError("Get_Localized_Dialogue_Entry could not find the key " + key, this.gameObject);
            return "";
        }

        return Localized_Dialogue_Dictionaries[LocalizationManager.Language][key];
    }


    // Setting the delay for printing out the next character in the dialogue window.
    public void Text_Scroll_Speed_Change(float new_speed)
    {
        //UIManager.ui_manager.text_scroll_speed_slider.value = new_speed;
        text_scroll_speed = new_speed;
        PlayerPrefs.SetFloat("TextScrollSpeed", new_speed);
        PlayerPrefs.Save();
    }
    public void Text_Autosize_Change(bool toggle)
    {/*
        PlayerPrefs.SetInt("TextAutosize", System.Convert.ToInt32(toggle));
        //UIManager.ui_manager.text_autosize.isOn = toggle;
        if (toggle)
        {
            // Turn on best fit
            UIManager.ui_manager.text_panel.resizeTextForBestFit = true;
            UIManager.ui_manager.text_size_slider.interactable = false;
        }
        else
        {
            // Turn off best fit for manual sizing
            UIManager.ui_manager.text_panel.resizeTextForBestFit = false;
            UIManager.ui_manager.text_size_slider.interactable = true;
        }
        PlayerPrefs.Save();*/
    }
    public void Text_Size_Change(float new_text_size)
    {
        text_size = (int)new_text_size;
        //UIManager.ui_manager.text_size_slider.value = text_size;
        PlayerPrefs.SetInt("TextSize", text_size);
        UIManager.ui_manager.text_panel.fontSize = text_size;
        PlayerPrefs.Save();
    }
}
