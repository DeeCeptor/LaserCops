using UnityEngine;
using System.Collections.Generic;

public class Tether : MonoBehaviour
{
    public static Tether tether;

    public float Damage = 1f;   // Damage done by the tether to enemies

    LineRenderer line;

    public Color standard_colour;
    public Color pulsating_colour;

    public float left_width = 0.1f;
    public float right_width = 0.1f;

    Color cur_left;
    Color cur_right;

    float cur_tether_switching_cooldown;
    float tether_switching_cooldown = 0.4f;

    public enum TetherMode { None, Destroy, Capture };
    public TetherMode cur_tether_mode = TetherMode.Destroy;

    public List<GameObject> tether_links;


    // TETHER GRAPHICS
    public int zigs = 140;      // How many particles
    public float speed = 1f;    // How quickly do they oscillate
    public float scale = 1.5f;

    Color primary_colour;
    Color secondary_colour;

    public Color primary_destroy_colour = Color.red;
    public Color secondary_destroy_colour = Color.white;
    public Color primary_capture_colour = Color.blue;
    public Color secondary_capture_colour = Color.white;

    Perlin noise;
    float oneOverZigs;

    private Particle[] particles;
    List<GameObject> links;

    ParticleEmitter particle_emitter;


    void Awake ()
    {
        tether = this;
        line = this.GetComponent<LineRenderer>();
        particle_emitter = this.GetComponent<ParticleEmitter>();
    }
	void Start ()
    {
        cur_left = standard_colour;
        cur_right = pulsating_colour;
        line.SetColors(cur_left, cur_right);
        line.SetWidth(left_width, right_width);

        SetTetherMode(TetherMode.Destroy);

        oneOverZigs = 1f / (float)zigs;
        GetComponent<ParticleEmitter>().emit = false;

        GetComponent<ParticleEmitter>().Emit(zigs);
        particles = particle_emitter.particles;
    }

    int num_players_holding_down_tether_button;
    public void TetherHeldDown()
    {
        // If both players are holding down the tether, disable it
        if (num_players_holding_down_tether_button > 1)
            DisableTether();
    }
    public void TetherReleased()
    {
        EnableTether();
    }
    public void DisableTether()
    {
        cur_tether_mode = TetherMode.None;
        foreach (GameObject obj in links)
        {
            obj.SetActive(false);
        }
    }
    public void EnableTether()
    {
        foreach (GameObject obj in links)
        {
            obj.SetActive(true);
        }
    }
    public void SwitchTether()
    {
        if (cur_tether_switching_cooldown <= 0)
        {
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
        cur_tether_switching_cooldown -= Time.deltaTime;
        // Pulsates between the 2 colours, end from end
        //line.SetColors()

        UpdateTetherGraphics();

        // Turn on/off tether based on its mode
        if (cur_tether_mode == TetherMode.None)
            particle_emitter.gameObject.SetActive(false);
        else
            particle_emitter.gameObject.SetActive(true);
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

        GetComponent<ParticleEmitter>().particles = particles;
    }
}
