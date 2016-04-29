using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Tether : MonoBehaviour
{
    public static Tether tether;

    public float Damage = 0.7f;   // Damage done by the tether to enemies

    LineRenderer line;

    public Color standard_colour;
    public Color pulsating_colour;

    public float left_width = 0.1f;
    public float right_width = 0.1f;

    float cur_tether_switching_cooldown;
    float tether_switching_cooldown = 0.4f;

    public enum TetherMode { None, Destroy, Capture };
    public TetherMode cur_tether_mode = TetherMode.Destroy;
    public TetherMode prev_tether_mode = TetherMode.Destroy;

    public GameObject tether_links_parent;
    public List<GameObject> tether_links;
    public GameObject middle_link;

    // TETHER GRAPHICS
    public int zigs = 300;      // How many particles
    public float speed = 1f;    // How quickly do they oscillate
    public float scale = 1.5f;

    public Color primary_colour;
    public Color secondary_colour;

    public Color primary_destroy_colour = Color.red;
    public Color secondary_destroy_colour = Color.white;
    public Color primary_capture_colour = Color.blue;
    public Color secondary_capture_colour = Color.white;

    Perlin noise;
    float oneOverZigs;

    private Particle[] particles;
    //List<GameObject> links;

    public ParticleEmitter particle_emitter;


    void Awake ()
    {
        tether = this;
        //line = tether_links_parent.GetComponent<LineRenderer>();
    }
	void Start ()
    {
        /*
        cur_left = standard_colour;
        cur_right = pulsating_colour;
        line.SetColors(cur_left, cur_right);
        line.SetWidth(left_width, right_width);
        */

        SetTetherMode(TetherMode.Destroy);

        oneOverZigs = 1f / (float)zigs;
        particle_emitter.emit = false;

        particle_emitter.Emit(zigs);
        particles = particle_emitter.particles;

        StartCoroutine(delayed_Start());
    }
    IEnumerator delayed_Start()
    {
        yield return new WaitForSeconds(0.1f);
        middle_link = tether_links[tether_links.Count / 2];
        UIManager.ui_manager.setMultiplierText();
    }

    int num_players_holding_down_tether_button;
    // Called when the player is holding down the button
    public void TetherHeldDown()
    {
        num_players_holding_down_tether_button++;

        // If both players are holding down the tether, disable it
        if (num_players_holding_down_tether_button > 0)
            DisableTether();
    }
    public void TetherReleased()
    {
        EnableTether();
    }
    public void DisableTether()
    {
        if (cur_tether_mode != TetherMode.None)
        {
            cur_tether_mode = TetherMode.None;

            tether_links_parent.SetActive(false);
            /*
            foreach (GameObject obj in tether_links)
            {
                obj.SetActive(false);
            }*/
        }
    }
    public void EnableTether()
    {
        if (cur_tether_mode == TetherMode.None)
        {
            Debug.Log(prev_tether_mode);
            cur_tether_mode = prev_tether_mode;
            SetTetherMode(cur_tether_mode);

            HingeJoint2D jo = tether_links[0].GetComponents<HingeJoint2D>()[1];
            jo.autoConfigureConnectedAnchor = false;

            tether_links_parent.SetActive(true);
            /*
            foreach (GameObject obj in tether_links)
            {
                obj.SetActive(true);
            }*/
        }
    }
    public void SwitchTether()
    {
        if (cur_tether_mode != TetherMode.None && cur_tether_switching_cooldown <= 0)
        {
            prev_tether_mode = cur_tether_mode;
            cur_tether_switching_cooldown = tether_switching_cooldown;

            if (cur_tether_mode == TetherMode.Destroy)
                SetTetherMode(TetherMode.Capture);
            else if (cur_tether_mode == TetherMode.Capture)
                SetTetherMode(TetherMode.Destroy);
        }
    }
    public void SetTetherMode(TetherMode mode)
    {
        if (mode == TetherMode.Destroy)
        {
            primary_colour = primary_destroy_colour;
            secondary_colour = secondary_destroy_colour;
            SetTetherLayer("DestructiveTether");

            // Spawn sparks to show the tether has changed modes
            for (int x = 0; x < tether_links.Count; x += 2)
            {
                GameObject sparks = (GameObject)GameObject.Instantiate(Resources.Load("Graphics/Tether Change Sparks") as GameObject,
                    tether_links[x].transform.position, Quaternion.identity);
                sparks.GetComponent<ParticleSystem>().startColor = primary_destroy_colour;
                Destroy(sparks, 1.0f);
            }
        }
        else if (mode == TetherMode.Capture)
        {
            primary_colour = primary_capture_colour;
            secondary_colour = secondary_capture_colour;
            SetTetherLayer("CaptureTether");

            // Spawn sparks to show the tether has changed modes
            for (int x = 0; x < tether_links.Count; x += 2)
            {
                GameObject sparks = (GameObject)GameObject.Instantiate(Resources.Load("Graphics/Tether Change Sparks") as GameObject,
                    tether_links[x].transform.position, Quaternion.identity);
                sparks.GetComponent<ParticleSystem>().startColor = primary_capture_colour;
                Destroy(sparks, 1.0f);
            }
        }
        Debug.Log("Setting tether " + mode);
        cur_tether_mode = mode;

        SoundMixer.sound_manager.PlayShortSpark();
    }


    public GameObject GetRandomLink()
    {
        return tether_links[Random.Range(0, tether_links.Count)];
    }
    // Adds a new link to the rope
    public void AddLink()
    {
        int new_link_position = 2;

        GameObject segment = ((GameObject)Instantiate(RopeGenerator.rope_generator.emptyPrefab,
            tether_links[1].transform.position, 
            Quaternion.identity));
        segment.transform.parent = tether_links_parent.transform;
        tether_links.Insert(2, segment);

        // Set the new link to connect
        tether_links[new_link_position].GetComponent<HingeJoint2D>().connectedBody = tether_links[new_link_position + 1].GetComponent<Rigidbody2D>();
        // Set the first link to connect to the new link
        tether_links[new_link_position - 1].GetComponent<HingeJoint2D>().connectedBody = tether_links[new_link_position].GetComponent<Rigidbody2D>();

        // Set new link neighbours
        segment.GetComponent<Link>().all_segments = tether_links;
        segment.GetComponent<Link>().rope = RopeGenerator.rope_generator;
        segment.GetComponent<Link>().above = tether_links[new_link_position + 1];
        segment.GetComponent<Link>().below = tether_links[new_link_position - 1];
        segment.GetComponent<Link>().top_most = tether_links[0];
        segment.GetComponent<Link>().bottom_most = tether_links[tether_links.Count - 1];

        tether_links[new_link_position - 1].GetComponent<Link>().above = segment;
        tether_links[new_link_position + 1].GetComponent<Link>().below = segment;

        // Recalculate middle
        middle_link = tether_links[tether_links.Count / 2];

        UIManager.ui_manager.setMultiplierText();
    }


    public void SetTetherLayer(string layer_name)
    {
        foreach (GameObject link in tether_links)
        {
            link.layer = LayerMask.NameToLayer(layer_name);
        }
    }

    // Removes the tether and all its links
    public void DestroyTether()
    {
        if (this.gameObject != null)
            Destroy(this.gameObject);
    }

    void LateUpdate()
    {
        num_players_holding_down_tether_button = 0;
        cur_tether_switching_cooldown -= Time.deltaTime;
        // Pulsates between the 2 colours, end from end
        //line.SetColors()

        UpdateTetherGraphics();

        // Turn on/off tether based on its mode
        if (cur_tether_mode == TetherMode.None)
            particle_emitter.enabled = (false);
        else
            particle_emitter.enabled = (true);
    }


    public void UpdateTetherGraphics()
    {
        if (noise == null)
            noise = new Perlin();

        float timex = Time.time * speed * 0.1365143f;
        float timey = Time.time * speed * 1.21688f;
        //float timez = Time.time * speed * 2.5564f;

        for (int i = 0; i < particles.Length; i++)
        {
            int cur_link = (int)(((float)tether_links.Count) * ((float)i / (float)particles.Length));
            int prev_link = Mathf.Max(0, cur_link - 1);//(int)(((float)links.Count) * ((float)Mathf.Max(0, (i - 1)) / (float)particles.Length));
            float t = ((float)i % (float)tether_links.Count / (float)tether_links.Count);

            //Vector3 position = links[prev_link].transform.position * t + links[cur_link].transform.position * (1 - t);
            Vector2 position = Vector2.Lerp(tether_links[prev_link].transform.position,
                tether_links[cur_link].transform.position,
                t);
            //Debug.Log(i + " : " + position.x + " : between : " + links[prev_link].transform.position + links[cur_link].transform.position + t + " : prev: " + prev_link + " cur: " + cur_link);
            /*
            Vector3 position = Vector3.Lerp(
                links[(int)(((float)links.Count) * ((float)Mathf.Max(0, (i - 1))/ (float)particles.Length))].transform.position, 
                links[(int) (((float) links.Count) * ((float) i / (float) particles.Length))].transform.position, 
                oneOverZigs * (float)i);*/
            //Vector3 position = Vector3.Lerp(transform.position, target.position, oneOverZigs * (float)i);
            Vector2 offset = new Vector3(noise.Noise(timex + position.x, timex + position.y, 0),
                                        noise.Noise(timey + position.x, timey + position.y, 0),
                                        0);
            //position += (offset * scale * ((float)i * oneOverZigs));
            position += (offset * scale * ((float)20 * oneOverZigs));

            particles[i].velocity = (Vector2)particles[i].velocity;
            particles[i].position = position;
            particles[i].energy = 99999f;

            // Oscillate colour
            // Red / white is cool staticy look
            Color c = Color.Lerp(primary_colour, secondary_colour, Random.value);
            c.a = Random.value;
            particles[i].color = c;
            //particles[i].color = Color.white;
        }

        particle_emitter.particles = particles;
    }
}
