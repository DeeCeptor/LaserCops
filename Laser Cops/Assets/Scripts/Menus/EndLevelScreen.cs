using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EndLevelScreen : MonoBehaviour 
{
    public Rigidbody2D player_1;
    public Rigidbody2D player_2;

    public TextMesh text;

    public List<GameObject> p1_victory_objects = new List<GameObject>();
    public List<GameObject> p2_victory_objects = new List<GameObject>();

    public bool coop_victory = false;
    public bool coop_defeat = false;
    public bool competitive = false;
    public bool competitive_blue_wins = false;
    public bool competitive_pink_wins = false;

    public Color c1;
    public Color c2;

    float time_till_menu = 9f;

    void Awake ()
    {
        // Initial
        player_1.velocity = new Vector2(20, 0);
        foreach (GameObject obj in p1_victory_objects)
        {
            obj.SetActive(true);
        }
        player_2.velocity = new Vector2(20, 0);
        foreach (GameObject obj in p2_victory_objects)
        {
            obj.SetActive(true);
        }

        // Search for level result
        GameObject lr_obj = GameObject.Find("LevelResult");
        if (lr_obj != null)
        {
            Debug.Log("Found level result");
            LevelResult lr = lr_obj.GetComponent<LevelResult>();
            coop_victory = lr.coop_victory;
            coop_defeat = lr.coop_defeat;
            competitive = lr.competitive;
            competitive_blue_wins = lr.competitive_blue_wins;
            competitive_pink_wins = lr.competitive_pink_wins;
            Destroy(lr_obj);
        }
        else
            Debug.Log("Couldn't find level result");
    }
    void Start () 
	{
	
	}
	

    public void Defeat(GameObject obj)
    {
        SoundMixer.sound_manager.Play8bitExplosion();

        EffectsManager.effects.ViolentExplosion(obj.transform.position);
        EffectsManager.effects.GridExplosion(obj.transform.position, 2f, 9f, Color.red);

        //GameState.game_state.ChangeTimescale(0.3f);

        obj.gameObject.layer = LayerMask.NameToLayer("Dead Player");
        obj.gameObject.tag = "Obstacle";
        obj.AddComponent<PlayerDying>();

        obj.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        obj.GetComponent<Rigidbody2D>().gravityScale = 5.0f;
        obj.GetComponent<Rigidbody2D>().isKinematic = false;
    }


    public void VictoryP1()
    {
        player_1.velocity = Vector2.zero;
        foreach (GameObject obj in p1_victory_objects)
        {
            ParticleSystem ps = obj.GetComponent<ParticleSystem>();
            if (!ps)
            {
                ps.enableEmission = false;
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }
    public void VictoryP2()
    {
        player_2.velocity = Vector2.zero;
        foreach (GameObject obj in p2_victory_objects)
        {
            ParticleSystem ps = obj.GetComponent<ParticleSystem>();
            if (!ps)
            {
                ps.enableEmission = false;
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(EnteredTrigger());
    }
    IEnumerator EnteredTrigger()
    {
        if (coop_victory)
        {
            VictoryP1();
            VictoryP2();
            text.text = "Victory";
        }
        else if (coop_defeat)
        {
            text.text = "Defeat";
            Defeat(player_1.gameObject);
            Defeat(player_2.gameObject);
        }
        else if (competitive)
        {
            Debug.Log("Competitive");
            if (competitive_blue_wins)
            {
                VictoryP1();
                Defeat(player_2.gameObject);
                text.text = "Blue Wins";

            }
            else if (competitive_pink_wins)
            {
                VictoryP2();
                Defeat(player_1.gameObject);
                text.text = "Pink Wins";
            }
        }

        yield return new WaitForSeconds(1f);
        text.gameObject.SetActive(true);
    }


    public void Retry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(PlayerPrefs.GetString("LastLevelPlayed"));
    }
    public void ToMenu()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("GameMode");
        if (obj != null)
        {
            Debug.Log("Found game mode setting");
            Mode mode = obj.GetComponent<Mode>();
        }
    }


    void Update () 
	{
        text.color = Color.Lerp(c1, c2, Mathf.PingPong(Time.time, 1f));

        time_till_menu -= Time.deltaTime;
        if (time_till_menu <= 0)
        {
            Debug.Log("Switching levels");
            time_till_menu = 99999f;
            UnityEngine.SceneManagement.SceneManager.LoadScene("SceneSelect");
        }
    }
}
