using UnityEngine;
using System.Collections;
using MKGlowSystem;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public bool play_intro_cutscene = true;

    public static CameraManager cam_manager;
    public float desired_size = 7.0f;   // How large the camera is zooming to
    float zoom_speed = 0.5f;        // How quickly the zoom changes
    [HideInInspector]
    public Camera cam;
    BoxCollider2D camera_region;

    public LayerMask intro_culling_mask;
    int initial_culling_mask;   // Saves default value to restore later

    public Transform target_of_zoom;

    void Awake ()
    {
        public_Awake();
    }
    // Added to avoid error of gamestate calling needing this class too early
    public void public_Awake()
    {
        cam = this.GetComponent<Camera>();
        cam_manager = this;
        cam.orthographicSize = desired_size;
        //ChangeZoom(desired_size, 2f);// 0.5f);
        camera_region = this.GetComponent<BoxCollider2D>();
        initial_culling_mask = cam.cullingMask;
    }
    void Start ()
    {
        Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        camera_region.size = new Vector2((Mathf.Abs(minScreenBounds.x) + Mathf.Abs(maxScreenBounds.x)) / 1 * this.transform.localScale.x,
                                 (Mathf.Abs(minScreenBounds.y) + Mathf.Abs(maxScreenBounds.y)) / 1 * this.transform.localScale.y);

        if (play_intro_cutscene)
            StartCoroutine(Introduction_Cutscene());
        else
        {
            Debug.Log("No starting cutscene, enabling input");
            GameState.game_state.Toggle_Player_Input(true);
        }
    }





    // Played at the beginning of every leve
    IEnumerator Introduction_Cutscene()
    {
        // Disable glow on the main camera
        this.GetComponent<MKGlow>().enabled = false;
        cam.cullingMask = intro_culling_mask;
        Vector3 original_speed = SkyboxCamera.skybox_camera.GetComponent<Rotate>().rotation_speed;
        SkyboxCamera.skybox_camera.GetComponent<Rotate>().rotation_speed = new Vector3(10, 20, 0);
        UIManager.ui_manager.Level_Name.GetComponentInChildren<Text>().text = GameState.game_state.current_level_name;
        UIManager.ui_manager.Mode.GetComponent<Image>().sprite = GameState.game_state.GetModeSprite();

        // Disable input
        yield return 0;
        GameState.game_state.Toggle_Player_Input(false);

        yield return new WaitForSeconds(1.5f);
        UIManager.ui_manager.Level_Name.SetActive(true);
        SoundMixer.sound_manager.PlayLong8bitLazer();

        yield return new WaitForSeconds(2f);
        UIManager.ui_manager.Mode.SetActive(true);
        SoundMixer.sound_manager.PlayNotification();

        yield return new WaitForSeconds(0.5f);
        // Set the colour of the difficulty
        UIManager.ui_manager.Difficulty.SetActive(true);
        Color c = Color.gray;
        switch (GameState.game_state.current_difficulty)
        {
            case GameState.Difficulty.Easy:
                c = new Color(102f/255f, 51f/255f, 0);
                break;
            case GameState.Difficulty.Normal:
                c = Color.gray;
                break;
            case GameState.Difficulty.Hard:
                c = Color.yellow;
                break;
        }
        UIManager.ui_manager.Difficulty.GetComponentInChildren<Text>().color = c;
        UIManager.ui_manager.Difficulty.GetComponentInChildren<Text>().text = GameState.game_state.current_difficulty.ToString();
        SoundMixer.sound_manager.PlayTransferHealth();

        // Create bright transition glow
        yield return new WaitForSeconds(1f);
        this.GetComponent<MKGlow>().enabled = true;
        EffectsManager.effects.glow.GlowType = MKGlowSystem.MKGlowType.Fullscreen;
        StartCoroutine(EffectsManager.effects.FlashScreenBriefly(1f));
        UIManager.ui_manager.Level_Name.AddComponent<FadeImage>();
        UIManager.ui_manager.Mode.AddComponent<FadeImage>();
        UIManager.ui_manager.Difficulty.AddComponent<FadeImage>();

        yield return new WaitForSeconds(0.5f);
        cam.cullingMask = initial_culling_mask;

        yield return new WaitForSeconds(0.25f);
        foreach (PlayerController p in GameState.game_state.Players)
        {
            //p.boosted_this_instant = true;
        }

        // Normal glow settings, transition is completely done
        yield return new WaitForSeconds(0.25f);
        SkyboxCamera.skybox_camera.GetComponent<Rotate>().rotation_speed = original_speed;
        EffectsManager.effects.glow.GlowType = MKGlowSystem.MKGlowType.Selective;
        EffectsManager.effects.glow.GlowIntensity = EffectsManager.effects.normal_glow_intensity;
        UIManager.ui_manager.Intro_UI.SetActive(false);

        // Enable player input
        GameState.game_state.Toggle_Player_Input(true);

        yield return null;
    }


    public void ChangeZoom(float zoom_level, float speed)
    {
        desired_size = zoom_level;
        zoom_speed = speed;
    }


    void Update ()
    {
        if (this.cam.orthographicSize != desired_size)
        {
            if (this.cam.orthographicSize < desired_size)
            {
                this.cam.orthographicSize = Mathf.Min(desired_size, this.cam.orthographicSize + Time.deltaTime * zoom_speed);
            }
            else
            {
                this.cam.orthographicSize = Mathf.Max(desired_size, this.cam.orthographicSize - Time.deltaTime * zoom_speed);
            }
        }

        if (target_of_zoom != null && this.transform.position != target_of_zoom.transform.position)
        {
            this.transform.position = Vector2.Lerp(this.transform.position, target_of_zoom.transform.position, Time.unscaledDeltaTime * 0.65f);
        }
    }
}
