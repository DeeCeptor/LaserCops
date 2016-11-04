using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameState : MonoBehaviour
{
    public static GameState game_state;

    public string level_to_load_on_victory = "SceneSelect";
    public string level_to_load_on_defeat = "SceneSelect";
    public string current_level_name = "Gettin' Pushy";     // Set in each level. Is also passed in from the level select
    public int number_of_players = 2;

    public bool paused = false; // Paused by player
    public bool game_over = false;  // Lost the game
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

    [HideInInspector]
    public bool tether_touching_obstacle = false;
    [HideInInspector]
    public float time_last_touched_obstacle;
    float turn_off_tether_touching_obstacle_time = 0.3f;

    public bool debug_invulnerability = false;
    bool debugging = true;
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


    void Awake()
    {
        game_state = this;

        SetGameMode();
        SetSettings();


        if (number_of_players > 2)
        {
            // Spawn player 3
            GameObject obj = Instantiate(Resources.Load("Players/Player 3") as GameObject);
            obj.transform.position = new Vector2(0, 1);
        }
        if (number_of_players > 3)
        {
            // Spawn player 4
            GameObject obj = Instantiate(Resources.Load("Players/Player 4") as GameObject);
            obj.transform.position = new Vector2(0, -1);
        }

        PlayerObjects = GameObject.FindGameObjectsWithTag("Player");
        if (VIP)
        {
            VIPObject = GameObject.FindGameObjectWithTag("VIP");
        }
    }
    void Start()
    {

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
                    Player_Health_Modifier = 0.7f;
                    Enemy_Health_Modifier = 1.4f;
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
            Destroy(obj);
        }
        else
        {
            // Didn't find a selected game mode, just used cooperative
            Debug.Log("Couldn't find game mode");
            Cooperative();
        }
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



    void Update()
    {
        
        if (Input.GetButtonDown("Pause") && !game_over)
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
            ResetVelocityPositionIterations();
        }

        if(VIP && VIPObject == null)
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
                Time.timeScale = 5f;
            }
            else if (Time.timeScale > 1)
            {
                increased_speed = false;
                Time.timeScale = 1;
            }
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


    // Checks if this is a game over
    public void CheckGameOver()
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
                    if (Players.Count <= 1)
                    {
                        GameOver();
                    }
                    break;
            }
        }
    }
    public void GameOver()
    {
        if (!game_over)
        {
            Debug.Log("You lose!");
            game_over = true;
            //InGameUIManager.ui_manager.SetAnnouncementText("You lost!", 9999);

            LevelResult lr = SpawnLevelResult();
            if (game_mode == GameMode.Competitive)
            {
                Debug.Log("Competitive");

                lr.competitive = true;
            }
            else
            {
                Debug.Log("Not competitive");
                lr.coop_victory = false;
                lr.coop_defeat = true;
                lr.competitive = false;
            }

            ChangeScene(2f, "EndLevelScreen");//level_to_load_on_defeat);
        }
    }
    public void Victory(string text = "You beat the level!")
    {
        if (!game_over)
        {
            Debug.Log(text + " on " + this.game_mode + " " + this.current_difficulty);
            game_over = true;

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

            LevelResult lr = SpawnLevelResult();
            if (game_mode == GameMode.Competitive)
            {
                lr.competitive = true;
                lr.coop_victory = false;
                lr.coop_defeat = false;

                foreach (PlayerController p in Players)
                {
                    if (p.player_number == 1)
                    {
                        if (p.alive)
                        {
                            lr.competitive_blue_wins = true;
                        }
                    }
                    else if (p.player_number == 2)
                    {
                        if (p.alive)
                        {
                            lr.competitive_pink_wins = true;
                        }
                    }
                }
            }
            else
            {
                lr.coop_victory = true;
                lr.coop_defeat = false;
            }

            ChangeScene(2f, "EndLevelScreen");//level_to_load_on_victory);
        }
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
        StartCoroutine(loadMenu(delay, scene_to_load));
    }
    public IEnumerator loadMenu(float delay, string scene_to_load)
    {
        yield return new WaitForSeconds(delay);
        ChangeTimescale(1f);
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
        Debug.Log("Pausing");
        Time.timeScale = 0;
        InGameUIManager.ui_manager.pause_menu.SetActive(true);
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
}