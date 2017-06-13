using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MKGlowSystem;
using System;
using Steamworks;
using InControl;

public class GameState : MonoBehaviour
{
    public static GameState game_state;

    public string level_to_load_on_victory = "SceneSelect";
    public string level_to_load_on_defeat = "SceneSelect";
    public string current_level_name = "Gettin' Pushy";     // Set in each level. Is also passed in from the level select
    public int number_of_players = 2;
    public List<List<string>> player_inputs;

    public bool overriding_no_tether = false;   // Set this to true if you never want the players to have a tether, regardless of mode

    public bool paused = false; // Paused by player
    public bool game_over = false;  // Lost the game
    public bool players_invuln = false;
    public float elapsed_game_time = 0f;
    public bool going_sideways;

    // Mode settings
    public GameMode game_mode = GameMode.Cooperative;
    public enum GameMode { Cooperative, Competitive, TetherOn, NoTether, OneHitKill, Chained, Gravity }
    public bool no_tether = false;
    public bool can_disable_tether = true;
    public bool can_change_tether_mode = true;
    public bool can_boost = true;
    public bool chained_to_center = false;
    public bool can_transfer_health = true;

    // Difficulty settings
    public Difficulty current_difficulty = Difficulty.Normal;
    public enum Difficulty {  Easy, Normal, Hard };
    public float Player_Health_Modifier = 1.0f; // Multiplies the player's max health (hard makes this number lower)
    public float Enemy_Health_Modifier = 1.0f;  // Multiplies each enemies' health (hard makes this number higher)

    public float inactive_speed = 1.0f;     // How quickly objects move when offscreen

    public List<PlayerController> Players = new List<PlayerController>();
    public GameObject[] PlayerObjects;
    public bool VIP = false;
    public GameObject VIPObject;
    
    public bool tether_touching_obstacle = false;
    //specifically for the spynet boss battle
    public bool tether_touching_obstacle_up = false;
    public float time_last_touched_obstacle;
    [HideInInspector]
    public bool limit_player_control_from_obstacles = false;
    float turn_off_tether_touching_obstacle_time = 0.3f;

    public bool debug_invulnerability = false;
    bool debugging = false;
    bool show_debugging_text = false;
    bool increased_speed = false;
    float normal_physics_delta_time;

    int default_velocity_iterations;
    int default_position_iterations;
    public static int intensive_velocity_iterations = 900;
    public static int intensive_position_iterations = 900;

    public Sprite coop_icon;
    public Sprite competitive_icon;
    public Sprite chained_icon;
    public Sprite one_hit_kill_icon;
    public Sprite tether_on_icon;
    public Sprite tether_off_icon;

    float delay_between_level_switching = 1.72f;
    float slowdown_factor = 0.4f;

    float prev_effects_volume;

    public bool sparksOff = false;

    // Use when bullets are reflected
    public Material default_sprite_material;

    // OPTIONAL:  If not empty, players will be positioned based on the positions of these objects. Goes in player order: P1,P2,P3,P4
    public List<GameObject> starting_positions = new List<GameObject>();

    //wheter there are any obstacles in the level this is important for setting the physics iteration
    public bool obstacle_level = true;


    void Awake()
    {
        game_state = this;
        game_over = false;

        if (Application.isEditor)
            debugging = true;

        SetGameMode();
        SetSettings();

        PlayerObjects = GameObject.FindGameObjectsWithTag("Player");
        if (VIP)
        {
            VIPObject = GameObject.FindGameObjectWithTag("VIP");
        }

        if (CameraManager.cam_manager == null)
        {
            Camera.main.GetComponent<CameraManager>().public_Awake();
        }

        // Set camera to be slightly more zoomed out when there are more players 
        if (number_of_players > 2)
        {
            CameraManager.cam_manager.desired_size *= 1.1f;
            CameraManager.cam_manager.cam.orthographicSize = CameraManager.cam_manager.desired_size;
        }
    }
    void Start()
    {
        //GameObject obj_ = (GameObject)Instantiate(Resources.Load("Enemies/Vehicles/MostBasicEnemy") as GameObject, new Vector3(1, 0, 0), Quaternion.identity);
        //obj_.GetComponent<basicScrollingEnemyScript>().Die();

        int x_pos = -4;

        // Arrange position of players
        if (starting_positions == null || starting_positions.Count < number_of_players)
        {
            switch (number_of_players)
            {
                case 2:
                    Get_Player(1).transform.position = VectorGrid.grid.transform.position + new Vector3(x_pos, 2, 0);
                    Get_Player(2).transform.position = VectorGrid.grid.transform.position + new Vector3(x_pos, -2, 0);
                    break;
                case 3:
                    Get_Player(1).transform.position = VectorGrid.grid.transform.position + new Vector3(x_pos, 3, 0);
                    Get_Player(2).transform.position = VectorGrid.grid.transform.position + new Vector3(x_pos, 0, 0);
                    Get_Player(3).transform.position = VectorGrid.grid.transform.position + new Vector3(x_pos, -3, 0);
                    break;
                case 4:
                    Get_Player(1).transform.position = VectorGrid.grid.transform.position + new Vector3(x_pos, 3, 0);
                    Get_Player(2).transform.position = VectorGrid.grid.transform.position + new Vector3(x_pos, 1, 0);
                    Get_Player(3).transform.position = VectorGrid.grid.transform.position + new Vector3(x_pos, -1, 0);
                    Get_Player(4).transform.position = VectorGrid.grid.transform.position + new Vector3(x_pos, -3, 0);
                    break;
            }
        }
        // Use starting positions
        else if (starting_positions != null && starting_positions.Count >= number_of_players && number_of_players > 3)
        {
            Debug.Log("Using preset starting positions");
            switch (number_of_players)
            {
                case 2:
                    Get_Player(1).transform.position = starting_positions[0].transform.position;
                    Get_Player(2).transform.position = starting_positions[1].transform.position;
                    break;
                case 3:
                    Get_Player(1).transform.position = starting_positions[0].transform.position;
                    Get_Player(2).transform.position = starting_positions[1].transform.position;
                    Get_Player(3).transform.position = starting_positions[2].transform.position;
                    break;
                case 4:
                    Get_Player(1).transform.position = starting_positions[0].transform.position;
                    Get_Player(2).transform.position = starting_positions[1].transform.position;
                    Get_Player(3).transform.position = starting_positions[2].transform.position;
                    Get_Player(4).transform.position = starting_positions[3].transform.position;
                    break;
            }
        }

        // Zoom out highwaygrid if there are more than 2 players
        if (number_of_players > 2)
        {
            Vector3 scale = VectorGrid.grid.transform.localScale;
            scale.x *= 1.1f;
            VectorGrid.grid.transform.localScale = scale;
        }
    }

    public GameObject Get_Player(int number)
    {
        foreach (GameObject obj in PlayerObjects)
        {
            if (obj.GetComponent<PlayerController>().player_number == number)
                return obj;
        }
        Debug.Log("Couldn't find player: " + number, this.gameObject);
        return null;
    }

    public void SetGameMode()
    {
        // Look for the game mode selector
        GameObject obj = GameObject.FindGameObjectWithTag("GameMode");
        if (obj)
        {
            Debug.Log("Found game mode setting");
            Mode mode = obj.GetComponent<Mode>();
            current_level_name = mode.level_to_load;



            current_difficulty = mode.difficulty;
            switch (mode.difficulty)
            {
                case Difficulty.Normal:
                    Player_Health_Modifier = 1.0f;
                    Enemy_Health_Modifier = 1.0f;
                    break;
                case Difficulty.Hard:
                    Player_Health_Modifier = 0.8f;
                    Enemy_Health_Modifier = 1.2f;
                    break;
            }

            game_mode = mode.mode;
            switch (mode.mode)
            {
                case GameMode.Cooperative:
                    Cooperative();
                    break;
                case GameMode.Competitive:
                    Competitive();
                    break;
                case GameMode.NoTether:
                    NoTether();
                    break;
                case GameMode.TetherOn:
                    TetherOn();
                    break;
                case GameMode.OneHitKill:
                    OneHitKill();
                    break;
                case GameMode.Chained:
                    Chained();
                    break;
            }

            number_of_players = Mathf.Max(2, ControlsManager.Player_Controls.Count);//mode.player_inputs.Count;

            Destroy(obj);
        }
        else
        {
            // Didn't find a selected game mode, just used cooperative
            Debug.Log("Couldn't find game mode, going with what the current GameState is set to");
            switch (game_mode)
            {
                case GameMode.Cooperative:
                    Cooperative();
                    break;
                case GameMode.Competitive:
                    Competitive();
                    break;
                case GameMode.NoTether:
                    NoTether();
                    break;
                case GameMode.TetherOn:
                    TetherOn();
                    break;
                case GameMode.OneHitKill:
                    OneHitKill();
                    break;
                case GameMode.Chained:
                    Chained();
                    break;
            }
        }

        // Instantiate more players
        if (number_of_players > 2)
        {
            // Spawn player 3
            GameObject p = Instantiate(Resources.Load("Players/Player 3") as GameObject);
            p.transform.position = new Vector2(0, -1.5f);
        }
        if (number_of_players > 3)
        {
            // Spawn player 4
            GameObject p = Instantiate(Resources.Load("Players/Player 4") as GameObject);
            p.transform.position = new Vector2(0, 1.5f);
        }

        if (overriding_no_tether)
            no_tether = true;
    }
    public void Cooperative()
    {
        Debug.Log("SetCooperative");
        can_disable_tether = true;
        can_change_tether_mode = true;
        can_boost = true;
        chained_to_center = false;
        can_transfer_health = true;
    }
    public void Competitive()
    {
        Debug.Log("SetCompetitive");
        can_disable_tether = true;
        can_change_tether_mode = true;
        can_boost = true;
        chained_to_center = false;
        can_transfer_health = false;
    }
    public void NoTether()
    {
        Debug.Log("NoTether");
        can_disable_tether = false;
        can_change_tether_mode = false;
        can_boost = true;
        chained_to_center = false;
        can_transfer_health = true;
        no_tether = true;
    }
    public void TetherOn()
    {
        Debug.Log("TetherOn");
        can_disable_tether = false;
        can_change_tether_mode = true;
        can_boost = true;
        chained_to_center = false;
        can_transfer_health = true;
    }
    public void OneHitKill()
    {
        Debug.Log("OneHitKill");
        can_disable_tether = true;
        can_change_tether_mode = true;
        can_boost = true;
        chained_to_center = false;
        can_transfer_health = true;
    }
    public void Chained()
    {
        Debug.Log("Chained");
        can_disable_tether = false;
        can_change_tether_mode = true;
        can_boost = true;
        chained_to_center = true;
        can_transfer_health = true;
        no_tether = false;
    }

    // Sets physics and graphics stuff
    public void SetSettings()
    {
        // Vastly improves frame rate, making stars scrolling by not look terrible
        QualitySettings.vSyncCount = 0;

        // Set the correct gravity
        SetGravity(new Vector3(-9.81f, 0));

        normal_physics_delta_time = Time.fixedDeltaTime;
        		Application.targetFrameRate = 120;
        default_position_iterations = Physics2D.positionIterations;
        default_velocity_iterations = Physics2D.velocityIterations;
    }
    public void SetGravity(Vector2 new_gravity)
    {
        Physics2D.gravity = new_gravity;
        Physics.gravity = new_gravity;

        if (Mathf.Abs(new_gravity.x) >= Mathf.Abs(new_gravity.y))
            going_sideways = true;
        else
            going_sideways = false;
    }
    
    // set the number of physics iterations for the level
    public void setIterations()
    {
        if (!obstacle_level)
        {

        }
    }

    public void Heal_All_Players(float amount)
    {
        if (current_difficulty == Difficulty.Easy)
            amount *= 1.5f;

        GameObject[] playerObjects = GameState.game_state.PlayerObjects;
        for (int i = 0; i < playerObjects.Length; i++)
        {
            PlayerController playerScript = playerObjects[i].GetComponent<PlayerController>();
            playerScript.Heal(amount);
        }
    }


    public string getFormattedTime(float in_time)
    {
        int minutes = (int)((in_time) / 60.0f);
        int seconds = (int)(in_time % 60);
        int milliseconds = (int)((in_time - (minutes * 60) - seconds) * 100);
        return ("" + minutes).PadLeft(2, '0') + ":" + ("" + seconds).PadLeft(2, '0') + "." + ("" + milliseconds).PadLeft(2, '0');
    }


    public void PlayerHitDeathzone()
    {
        if (GameMode.Competitive != game_mode)
        {
            for (int x = 0; x < Players.Count; x++)
            {
                Players[x].Die();
            }
        }
    }


    public void Boss_Died()
    {
        Debug.Log("Boss has died");
        players_invuln = true;
    }


    // Checks if this is a game over
    public void CheckGameOver(PlayerController player_who_just_died = null)
    {
        if (!game_over)
        {
            switch (game_mode)
            {
                case GameMode.Competitive:
                    if (Players.Count <= 1)
                    {
                        Victory("Player " + Players[0].player_number + " Wins!");
                    }
                    break;
                default:
                    if (Players.Count < number_of_players)
                    {
                        GameOver(player_who_just_died);
                    }
                    break;
            }
        }
    }
    public void GameOver(PlayerController player_who_just_died = null)
    {
        if (game_over)
            return;

        Debug.Log("You lose!");
        game_over = true;

        SoundMixer.sound_manager.DefeatFanfare();

        string text;
        if (game_mode == GameMode.Competitive)
        {
            Debug.Log("Competitive");
            text = "Competitive Defeat";
            //lr.competitive = true;
        }
        else
        {
            Debug.Log("Not competitive");
            text = "Coop Defeat";
            //lr.coop_victory = false;
            //lr.coop_defeat = true;
            //lr.competitive = false;
        }

        if (player_who_just_died != null)
        {
            CameraManager.cam_manager.target_of_zoom = player_who_just_died.transform;
            //VectorGrid.grid.m_GridWidth = 140;
            //VectorGrid.grid.transform.localScale = new Vector3(2, 2, 1);
        }

        End_Cutscene(text);

        ChangeScene(delay_between_level_switching, level_to_load_on_defeat);
    }
    public void Victory(string text = "You beat the level!", GameObject player_crossing_finish_line = null)
    {
        if (game_over)
            return;

        Debug.Log(text + " on " + this.game_mode + " " + this.current_difficulty);
        game_over = true;

        SoundMixer.sound_manager.VictoryFanfare();

        // Check for achievements
        VictoryCheckAchievements();

        // Slow down time
        //ChangeTimescale(0.5f);

        // Score fact that we beat level, on this mode
        PlayerPrefs.SetInt(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, 1);      // Beat level on anything
        PlayerPrefs.SetInt(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " " + this.current_difficulty, 1);  // Beat level on difficulty
        PlayerPrefs.SetInt(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " " + this.game_mode, 1);   // Beat level on mode
        PlayerPrefs.SetInt(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " " + this.game_mode + " " + this.current_difficulty, 1);   // Beat level on mode at difficulty

        // Record score
        int prev_high_score = PlayerPrefs.GetInt(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " High Score", 0);
        if (InGameUIManager.ui_manager.score >= prev_high_score)
        {
            PlayerPrefs.SetInt(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " High Score", InGameUIManager.ui_manager.score);
            // InGameUIManager.ui_manager.SetAnnouncementText("New High Score: " + InGameUIManager.ui_manager.score, 9999);
            Debug.Log("New high score : " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " " + InGameUIManager.ui_manager.score);
        }
        else
        {
            //InGameUIManager.ui_manager.SetAnnouncementText("Previous High Score: " + InGameUIManager.ui_manager.score, 9999);
            Debug.Log("No new high score: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " " + InGameUIManager.ui_manager.score);
        }

        //LevelResult lr = SpawnLevelResult();
        string text_to_display = "";
        if (game_mode == GameMode.Competitive)
        {
            /*
            lr.competitive = true;
            lr.coop_victory = false;
            lr.coop_defeat = false;
            */
            if (player_crossing_finish_line != null)
            {
                // Competitive, a player crossed the line (there's more than 1 alive), this crossing player wins
                PlayerController p = player_crossing_finish_line.GetComponent<PlayerController>();
                text_to_display = "Player " + p.player_number + " wins!";
                CameraManager.cam_manager.target_of_zoom = p.transform;
            }
            else
            {
                foreach (PlayerController p in Players)
                {
                    if (p.player_number == 1)
                    {
                        if (p.alive)
                        {
                            text_to_display = "Player " + p.player_number + " wins!";
                            CameraManager.cam_manager.target_of_zoom = p.transform;
                        }
                    }
                    else if (p.player_number == 2)
                    {
                        if (p.alive)
                        {
                            text_to_display = "Player " + p.player_number + " wins!";
                            CameraManager.cam_manager.target_of_zoom = p.transform;
                        }
                    }
                    else if (p.player_number == 3)
                    {
                        if (p.alive)
                        {
                            text_to_display = "Player " + p.player_number + " wins!";
                            CameraManager.cam_manager.target_of_zoom = p.transform;
                        }
                    }
                    else if (p.player_number == 4)
                    {
                        if (p.alive)
                        {
                            text_to_display = "Player " + p.player_number + " wins!";
                            CameraManager.cam_manager.target_of_zoom = p.transform;
                        }
                    }
                }
            }
        }
        else
        {
            //lr.coop_victory = true;
            //lr.coop_defeat = false;
            text_to_display = "Coop victory!";
        }

        End_Cutscene(text_to_display);
        if (player_crossing_finish_line != null)
        {
            CameraManager.cam_manager.target_of_zoom = player_crossing_finish_line.transform;
            VectorGrid.grid.m_GridWidth = 140;
        }

        ChangeScene(delay_between_level_switching, level_to_load_on_victory);
    }
    public void VictoryCheckAchievements()
    {
        // Check the game mode
        switch (game_mode)
        {
            case GameMode.Competitive:
                GetAchievement("Sweet Victory");
                break;
            case GameMode.Chained:
                GetAchievement("Ball & Chain");
                break;
            case GameMode.OneHitKill:
                GetAchievement("Master");
                if (current_difficulty == Difficulty.Hard)
                    GetAchievement("Impossible");
                break;
        }

        if (current_difficulty == Difficulty.Hard)
            GetAchievement("Likes a Challenge");

        if (number_of_players >= 4)
            GetAchievement("More the Merrier");

        // Check for specific levels
        switch (current_level_name)
        {
            case "Easing into It":
                GetAchievement("Easing into It");
                break;
            case "Disco Dan":
                GetAchievement("Disco Dan");
                break;
            case "Spy Net":
                GetAchievement("Spy Net");
                break;
            case "Gunship Gunther":
                GetAchievement("Gunship Gunther");
                break;
            case "Bonnie & Clyde":
                GetAchievement("Bonnie & Clyde");
                GetAchievement("Earned Your Badge");
                break;
        }
    }
    public void GetAchievement(string name)
    {
        if (SteamManager.Initialized)
        {
            try
            {
                bool successful = SteamUserStats.SetAchievement(name);
                if (successful)
                    Debug.Log("Got achievement: " + name);
                else
                    Debug.LogError("Did not get achievement: " + name);

                successful = SteamUserStats.StoreStats();
                if (successful)
                    Debug.Log("Stored stats after achievement: " + name);
                else
                    Debug.LogError("Did not store stats achievement: " + name);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message, this);
            }
        }
        else
            Debug.Log("SteamManager not initialized or achievement_name is empty");
    }


    // Play end cutscene (black and white, zoom in camera
    public void End_Cutscene(string text)
    {
        CameraManager.cam_manager.ChangeZoom(5f, 1.5f);
        GlowingBackgroundCamera.background_cam.GoGrayscale();
        InGameUIManager.ui_manager.end_of_level_text.SetActive(true);
        InGameUIManager.ui_manager.end_of_level_text.GetComponentInChildren<Text>().text = text;
    }

    public LevelResult SpawnLevelResult()
    {
        GameObject go = new GameObject();
        go.transform.name = "LevelResult";
        LevelResult lr = go.AddComponent<LevelResult>();
        return lr;
    }

    public void ChangeTimescale(float new_timescale)
    {
        Time.timeScale = new_timescale;
        Time.fixedDeltaTime = new_timescale * normal_physics_delta_time;
    }
    public void ResetVelocityPositionIterations()
    {
        Physics2D.velocityIterations = default_velocity_iterations;
        Physics2D.positionIterations = default_position_iterations;
    }
    public void SetNewDefaultVelocityPositionIterations(int velocity_iterations, int position_iterations)
    {
        default_position_iterations = position_iterations;
        default_velocity_iterations = velocity_iterations;
        ResetVelocityPositionIterations();
    }
    public void SetVelocityPositionIterations(int velocity_iterations, int position_iterations)
    {
        Physics2D.velocityIterations = velocity_iterations;
        Physics2D.positionIterations = position_iterations;
    }


    public void ChangeScene(float delay, string scene_to_load)
    {
        //Time.timeScale = 1;
        StartCoroutine(Fade_Out_Effects_Volume(1f));
        StartCoroutine(loadMenu(delay, scene_to_load));
    }
    public IEnumerator Fade_Out_Effects_Volume(float time)
    {
        prev_effects_volume = AudioManager.audio_manager.effects_volume;
        float total_vol_decrease = 0;
        while (time > 0)
        {
            total_vol_decrease += Time.deltaTime;
            //SoundMixer.sound_manager.EffectsVolumeChanged(AudioManager.audio_manager.effects_volume - total_vol_decrease);
            AudioManager.audio_manager.Effects_Volume_Changed_No_Saving(AudioManager.audio_manager.effects_volume - Time.deltaTime);
            time -= Time.deltaTime;
            yield return 0;
        }
    }
    public IEnumerator loadMenu(float delay, string scene_to_load)
    {
        yield return new WaitForSeconds(delay);

        ChangeTimescale(1f);
        AudioManager.audio_manager.Effects_Volume_Changed_No_Saving(prev_effects_volume);
        SoundMixer.sound_manager.StopAllSound();
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene_to_load);

        yield return null;
    }


    public void Unpause()
    {
        Debug.Log("Unpausing");
        Time.timeScale = 1;
        InGameUIManager.ui_manager.pause_menu.SetActive(false);
    }
    public void Pause()
    {
        if (!game_over)
        {
            Debug.Log("Pausing");
            Time.timeScale = 0;
            InGameUIManager.ui_manager.pause_menu.SetActive(true);
        }
    }


    public Sprite GetModeSprite()
    {
        switch (GameState.game_state.game_mode)
        {
            case GameState.GameMode.Cooperative:
                return coop_icon;
            case GameState.GameMode.Competitive:
                return competitive_icon;
            case GameState.GameMode.Chained:
                return chained_icon;
            case GameState.GameMode.OneHitKill:
                return one_hit_kill_icon;
            case GameState.GameMode.NoTether:
                return tether_off_icon;
            case GameState.GameMode.TetherOn:
                return tether_on_icon;
        }

        return coop_icon;
    }


    public void Toggle_Player_Input(bool enabled)
    {
        foreach (PlayerController p in Players)
        {
            p.input_enabled = enabled;
        }
    }


    void OnGUI()
    {
        if (debugging)
        {
            if (show_debugging_text)
            {
                if (debug_invulnerability)
                    GUI.Label(new Rect(0, 0, 300, 100), "Invulnerable");

                GUI.Label(new Rect(0, 10, 300, 100), Time.timeScale + "X");
                GUI.Label(new Rect(0, 20, 300, 100), "Length:" + Tether.tether.tether_links.Count);
                GUI.Label(new Rect(0, 30, 300, 100), "Score for new link:" + InGameUIManager.ui_manager.score_for_a_link);
            }
        }
    }



    float blur_timer;
    void Update()
    {
        if (game_over)
        {
            blur_timer -= Time.unscaledDeltaTime;
            if (blur_timer < 0)
            {
                CameraManager.cam_manager.GetComponent<MKGlow>().BlurIterations = Mathf.Min(CameraManager.cam_manager.GetComponent<MKGlow>().BlurIterations + 1, 7);
                blur_timer = 0.6f;
            }

            // Slow down the game
            this.ChangeTimescale(Mathf.Max(Time.timeScale - Time.unscaledDeltaTime * slowdown_factor, 0.1f));
        }

        if ((Input.GetButtonDown("Pause") || (InputManager.ActiveDevice !=  null && InputManager.ActiveDevice.CommandWasPressed) || (Time.timeScale == 0 && Input.GetButtonDown("Cancel"))) && !game_over)
        {
            if (Time.timeScale == 0)
            {
                // Currently paused, unpause game
                Unpause();
            }
            else
            {
                // Currently unpaused, pause game
                Pause();
            }
        }

        // Not paused and not game over
        if (Time.timeScale != 0 && !game_over)
        {
            // Timer is running if we're not paused
            elapsed_game_time += Time.deltaTime;
        }

        // Is the tether touching an obstacle?
        if (tether_touching_obstacle &&
            time_last_touched_obstacle + turn_off_tether_touching_obstacle_time < Time.time)
        {
            tether_touching_obstacle = false;
            tether_touching_obstacle_up = false;
            limit_player_control_from_obstacles = false;
            ResetVelocityPositionIterations();
        }

        if (VIP && VIPObject == null)
        {
            GameOver();
        }

        if (debugging)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                debug_invulnerability = !debug_invulnerability;
            }

            if (Input.GetKey(KeyCode.BackQuote))
            {
                increased_speed = true;
                debug_invulnerability = true;
                ChangeTimescale(5f);
                //Time.timeScale = 5f;
            }
            else if (Time.timeScale > 1)
            {
                increased_speed = false;
                ChangeTimescale(1f);
                //Time.timeScale = 1;
            }
        }
    }
}