using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// This class is created everytime the save button is clicked.
// A list of these SaveFiles is saved by SaveManager to the user's hard drive, and loaded when needed
[System.Serializable]
public class SaveFile 
{
    public string current_scene;

    // Current conversation and node
    public string current_conv;
    int current_conv_node;

    // Date saved
    public DateTime time_saved;
    // Time played, in seconds
    public float time_played;

    public string log_text;

    // Save all the actors on the scene
    List<string> left_actors = new List<string>();
    List<string> right_actors = new List<string>();

    // Background image     (image must be stored in Resources/Backgrounds)
    public string background_image_name;

    // Background music     (clip must be stored in Resources/Music)
    string background_music_name;

    // Keep track of which conversations are still on the scene (delete any conversations that have been deleted)
    public List<string> remaining_conversations = new List<string>();

    // Saved current stats
    public Dictionary<string, float> saved_numbered_stats;
    public Dictionary<string, bool> saved_boolean_stats;
    public Dictionary<string, string> saved_string_stats;
    public List<string> saved_items;

    public SaveFile()
    {
        
    }



    // Loads a scene and sets the settings of this save file to the game
    // Must be called with a monobehaviour with StartCoroutine(Load())
    // Modify this method to add in your own custom loading code
    public IEnumerator Load()
    {
        string active_scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // UI is unresponsive if there are 2 event systems
        if (UnityEngine.EventSystems.EventSystem.current.gameObject)
            GameObject.Destroy(UnityEngine.EventSystems.EventSystem.current.gameObject);

        // Load items
        StatsManager.items = saved_items;

        // Load the scene that was saved
        UnityEngine.SceneManagement.SceneManager.LoadScene(current_scene, UnityEngine.SceneManagement.LoadSceneMode.Additive);

        // Must wait 1 frame before initializing the new scene
        yield return 0;

        // Unpause in case the game was paused before loading
        Pause.pause.ResumeGame();

        // Stop the default things from happening
        VNSceneManager.scene_manager.starting_conversation = null;
        ConversationManager cur_conv = GameObject.Find(current_conv).GetComponent<ConversationManager>();
        cur_conv.Start_Conversation_Partway_Through(current_conv_node);

        // Set log
        VNSceneManager.scene_manager.Add_To_Log("", log_text);

        // Set play time
        VNSceneManager.scene_manager.play_time += time_played;

        ActorManager.actors_on_scene = new List<Actor>();
        ActorManager.exiting_actors = new List<Actor>();
        ActorManager.left_actors = new List<Actor>();
        ActorManager.right_actors = new List<Actor>();

        // Load the actors on the scene
        foreach (string a in left_actors)
        {
            ActorManager.Instantiate_Actor(a, Actor_Positions.LEFT);
        }
        foreach (string a in right_actors)
        {
            ActorManager.Instantiate_Actor(a, Actor_Positions.RIGHT);
        }


        // Backgrounds MUST be stored in Resources/Backgrounds
        if (!String.IsNullOrEmpty(background_image_name))
        {
            try
            {
                UIManager.ui_manager.background.gameObject.SetActive(true);
                UIManager.ui_manager.background.overrideSprite = Resources.Load<Sprite>("Backgrounds/" + background_image_name);
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading Background image " + background_image_name + ". Make sure your image is placed in the Resources/Backgrounds folder. " + e.Message);
            }
        }

        // Load background music
        if (!String.IsNullOrEmpty(background_music_name))
        {
            try
            {
                Debug.Log("Loading music: " + background_music_name);
                AudioManager.audio_manager.Set_Music(Resources.Load<AudioClip>("Music/" + background_music_name));
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading Background music " + background_music_name + ". Make sure your music is placed in the Resources/Music folder. " + e.Message);
            }
        }

        // Delete conversations not present in our saved conversations
        ConversationManager[] convs = (ConversationManager[])UnityEngine.Object.FindObjectsOfType(typeof(ConversationManager)) as ConversationManager[];
        foreach (ConversationManager c in convs)
        {
            // Find all conversations in our freshly loaded scene, check if we should keep or delete these conversations
            string name = (c.name);

            // Record the full name of the object (including its parents)
            Transform parent = c.transform.parent;
            while (parent != null)
            {
                name = name.Insert(0, parent.name + "/");
                parent = parent.parent;
            }


            bool delete_conv = true;
            // Check against saved conversations
            foreach (string s in remaining_conversations)
            {
                if (name == s)
                {
                    delete_conv = false;
                    break;
                }
            }

            if (delete_conv)
                GameObject.Destroy(c.gameObject);
        }

        // Load stats
        StatsManager.boolean_stats = saved_boolean_stats;
        StatsManager.numbered_stats = saved_numbered_stats;
        StatsManager.string_stats = saved_string_stats;
        StatsManager.Print_All_Stats();




        //<< MODIFY THIS SECTION TO LOAD THINGS SPECIFIC TO YOUR GAME >>//







        //<< MODIFY THE ABOVE SECTION TO LOAD THINGS SPECIFIC TO YOUR GAME >>//




        yield return 0;
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName(current_scene));
        UnityEngine.SceneManagement.SceneManager.UnloadScene(active_scene);
        AudioListener.pause = false;
        yield return 0;
    }




    // Set save information
    public void Save()
    {
        // Record game stats we must save (listed at top of file)
        log_text = UIManager.ui_manager.log_text.text;
        current_scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        //current_conversation = VNSceneManager.current_conversation;
        current_conv = SaveManager.GetGameObjectPath(VNSceneManager.current_conversation.transform);
        current_conv_node = VNSceneManager.current_conversation.cur_node;

        time_saved = DateTime.Now;
        time_played = VNSceneManager.scene_manager.play_time;

        //bg_music = AudioManager.audio_manager.background_music_audio_source.clip.;

        // Save the actors on the scene
        foreach (Actor a in ActorManager.left_actors)
        {
            left_actors.Add(a.actor_name);
        }
        foreach (Actor a in ActorManager.right_actors)
        {
            right_actors.Add(a.actor_name);
        }

        // Save the background image
        if (UIManager.ui_manager.background != null)
        {
            background_image_name = UIManager.ui_manager.background.overrideSprite.name;
        }
        else
        {
            Debug.LogError("Can't save BG image");
        }


        // Save the background music (if any)
        if (AudioManager.audio_manager.background_music_audio_source.clip)
        {
            background_music_name = AudioManager.audio_manager.background_music_audio_source.clip.name;
        }

        // Record all remaining conversations (deleted ones will not be recorded)
        ConversationManager[] convs = (ConversationManager[]) UnityEngine.Object.FindObjectsOfType(typeof(ConversationManager)) as ConversationManager[];
        foreach (ConversationManager c in convs)
        {
            string name = (c.name);

            // Record the full name of the object (including its parents)
            Transform parent = c.transform.parent;
            while (parent != null)
            {
                name = name.Insert(0, parent.name + "/");
                parent = parent.parent;
            }

            remaining_conversations.Add(name);
        }

        // Save stats
        saved_boolean_stats = StatsManager.boolean_stats;
        saved_numbered_stats = StatsManager.numbered_stats;
        saved_string_stats = StatsManager.string_stats;
        saved_items = StatsManager.items;




        //<< MODIFY THIS SECTION TO SAVE THINGS SPECIFIC TO YOUR GAME >>//











        //<< MODIFY THE ABOVE SECTION TO SAVE THINGS SPECIFIC TO YOUR GAME >>//




        SaveManager.AddNewSave(this);
    }
}
