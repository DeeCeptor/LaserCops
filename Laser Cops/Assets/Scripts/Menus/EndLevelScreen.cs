using UnityEngine;
using System.Collections.Generic;

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

    public Color c1;
    public Color c2;

    float time_till_menu = 6f;

    void Awake ()
    {
        // Initial
        player_1.velocity = new Vector2(15, 0);
        foreach (GameObject obj in p1_victory_objects)
        {
            obj.SetActive(true);
        }
        player_2.velocity = new Vector2(15, 0);
        foreach (GameObject obj in p2_victory_objects)
        {
            obj.SetActive(true);
        }


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

    }
    public void VictoryP2()
    {

    }


    void OnTriggerEnter2D(Collider2D other)
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

        }
    }


    void Update () 
	{
        //Color.Lerp( Mathf.PingPong(Time.time, 1f);

        time_till_menu -= Time.deltaTime;
        if (time_till_menu <= 0)
        {
            Debug.Log("Switching levels");
            time_till_menu = 99999f;
            UnityEngine.SceneManagement.SceneManager.LoadScene("SceneSelect");
        }
    }
}
