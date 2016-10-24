using UnityEngine;
using System.Collections;

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


    void Awake ()
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
            // If not playing cutscene, allow input
            foreach (PlayerController p in GameState.game_state.Players)
            {
                p.input_enabled = true;
            }
        }
    }


    // Played at the beginning of every leve
    IEnumerator Introduction_Cutscene()
    {
        cam.cullingMask = intro_culling_mask;

        yield return new WaitForSeconds(1.5f);
        UIManager.ui_manager.Level_Name.SetActive(true);

        yield return new WaitForSeconds(2.5f);
        UIManager.ui_manager.Mode.SetActive(true);

        yield return new WaitForSeconds(1f);
        EffectsManager.effects.glow.GlowType = MKGlowSystem.MKGlowType.Fullscreen;
        StartCoroutine(EffectsManager.effects.FlashScreenBriefly(1f));
        UIManager.ui_manager.Level_Name.AddComponent<FadeImage>();
        UIManager.ui_manager.Mode.AddComponent<FadeImage>();

        yield return new WaitForSeconds(0.5f);
        cam.cullingMask = initial_culling_mask;

        yield return new WaitForSeconds(0.25f);
        foreach (PlayerController p in GameState.game_state.Players)
        {
            p.boosted_this_instant = true;
        }

        yield return new WaitForSeconds(0.25f);
        EffectsManager.effects.glow.GlowType = MKGlowSystem.MKGlowType.Selective;
        EffectsManager.effects.glow.GlowIntensity = EffectsManager.effects.normal_glow_intensity;
        UIManager.ui_manager.Intro_UI.SetActive(false);
        // Enable player input
        foreach (PlayerController p in GameState.game_state.Players)
        {
            p.input_enabled = true;
        }

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
	}
}
