using UnityEngine;
using System.Collections.Generic;
using System.Collections;

//most of this was copied from our default tether if there is an error something unfortunate was probably left in
public class EnemyBossTetherScript : MonoBehaviour
{
    public static EnemyBossTetherScript EnemyTether;

    public float Damage = 0.5f;   // Damage done by the tether to enemies

    LineRenderer line;

    public Color standard_colour;
    public Color pulsating_colour;

    public float left_width = 0.1f;
    public float right_width = 0.1f;

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


    public Transform rope_pieces_parent;
    public HingeJoint2D anchor;

    //public string line_layer;
    //public List<GameObject> joints;
    private float NTDistance;
    public GameObject emptyPrefab;
    public GameObject beginning_anchor;
    public GameObject end_anchor;

    public GameObject beginning_rope_piece;
    public GameObject end_rope_piece;

    public Vector2 direction;// = new Vector2(1, 0);

    public int number_of_segments = 40;
    //public float size_of_rope_pieces = 0.2f;
    //public List<PlatformerCharacter2D> players_on_rope = new List<PlatformerCharacter2D>();

    public Material tether_material;


    void Awake()
    {
        EnemyTether = this;
        //line = tether_links_parent.GetComponent<LineRenderer>();
    }
    void Start()
    {
        /*
        cur_left = standard_colour;
        cur_right = pulsating_colour;
        line.SetColors(cur_left, cur_right);
        line.SetWidth(left_width, right_width);
        */

        oneOverZigs = 1f / (float)zigs;
        particle_emitter.emit = false;

        particle_emitter.Emit(zigs);
        particles = particle_emitter.particles;

        if (GameState.game_state.PlayerObjects.Length > 2)
        {
            number_of_segments = number_of_segments + (int)((float)number_of_segments * (float)((float)(GameState.game_state.PlayerObjects.Length - 2) / 2f));
            Debug.Log("Extending tether " + number_of_segments);
        }

        Generate_Rope_Between_Anchors();
        CalculateMiddleLink();

    }


    public void CalculateMiddleLink()
    {
        middle_link = tether_links[tether_links.Count / 2];

        if (GameState.game_state.chained_to_center)
        {
            anchor.connectedBody = middle_link.GetComponent<Rigidbody2D>();
        }
    }
    public void Generate_Rope_Between_Anchors()
    {
        /*if (beginning_anchor == null || end_anchor == null)
        {
            beginning_anchor = gameObject;
            end_anchor = gameObject;
        }*/
        tether_links = new List<GameObject>();
        //line = GetComponent<LineRenderer>();
        // vertexCount = (((int)Vector2.Distance(beginning.transform.position, end.transform.position)) * 3) - 1;

        //line.SetWidth(0.1f, 0.1f);  // 0.05f
        //line.sortingLayerName = line_layer;
        Vector3 dir = beginning_anchor.transform.position - end_anchor.transform.position;

        for (int i = 0; i < number_of_segments; i++)
        {
            GameObject segment = ((GameObject)Instantiate(emptyPrefab,
                new Vector3(beginning_anchor.transform.position.x, beginning_anchor.transform.position.y, 0) - ((dir / (float)number_of_segments) * i), Quaternion.identity));
            tether_links.Add(segment);
            segment.transform.parent = rope_pieces_parent;
        }

        // Connect all the tether_links and and make their parents this object
        for (int j = 0; j < tether_links.Count - 1; j++)
        {
            //tether_links[j].transform.parent = this.transform;
            tether_links[j].GetComponent<HingeJoint2D>().connectedBody = tether_links[j + 1].GetComponent<Rigidbody2D>();
        }

        // Set their neighbours
        for (int x = 0; x < tether_links.Count; x++)
        {
            int above_int = Mathf.Clamp(x - 1, 0, tether_links.Count - 1);
            int below_int = Mathf.Clamp(x + 1, 0, tether_links.Count - 1);
            tether_links[x].GetComponent<EnemyLink>().above = tether_links[above_int];
            tether_links[x].GetComponent<EnemyLink>().below = tether_links[below_int];
            tether_links[x].GetComponent<EnemyLink>().top_most = tether_links[0];
            tether_links[x].GetComponent<EnemyLink>().bottom_most = tether_links[tether_links.Count - 1];
            tether_links[x].GetComponent<EnemyLink>().position_from_top_in_rope = x;
            tether_links[x].GetComponent<EnemyLink>().position_from_bottom_in_rope = tether_links.Count - x;
            tether_links[x].GetComponent<EnemyLink>().all_segments = tether_links;
            tether_links[x].GetComponent<EnemyLink>().rope = this;
        }

        // Set connections on ends
        // Where player is
        HingeJoint2D jo = tether_links[0].AddComponent<HingeJoint2D>();
        //jo.autoConfigureConnectedAnchor = false;
        jo.connectedBody = beginning_anchor.GetComponent<Rigidbody2D>();
        beginning_rope_piece = tether_links[0];

        jo = tether_links[tether_links.Count - 1].GetComponent<HingeJoint2D>();
        jo.anchor = new Vector2(0, 0);
        jo.connectedBody = end_anchor.GetComponent<Rigidbody2D>();
        end_rope_piece = tether_links[tether_links.Count - 1];
    }

    public GameObject GetRandomLink()
    {
        return tether_links[Random.Range(0, tether_links.Count)];
    }
    // Adds a new link to the rope
    public void AddLink()
    {
        //Debug.Log(tether_links.Count);

        if (GameState.game_state.no_tether)
            return;

        int new_link_position = 2;

        GameObject segment = ((GameObject)Instantiate(this.emptyPrefab,
            tether_links[1].transform.position,
            Quaternion.identity));
        segment.transform.parent = tether_links_parent.transform;
        tether_links.Insert(2, segment);

        // Set the new link to connect
        tether_links[new_link_position].GetComponent<HingeJoint2D>().connectedBody = tether_links[new_link_position + 1].GetComponent<Rigidbody2D>();
        // Set the first link to connect to the new link
        tether_links[new_link_position - 1].GetComponent<HingeJoint2D>().connectedBody = tether_links[new_link_position].GetComponent<Rigidbody2D>();

        // Set new link neighbours
        segment.GetComponent<EnemyLink>().all_segments = tether_links;
        segment.GetComponent<EnemyLink>().rope = this;
        segment.GetComponent<EnemyLink>().above = tether_links[new_link_position + 1];
        segment.GetComponent<EnemyLink>().below = tether_links[new_link_position - 1];
        segment.GetComponent<EnemyLink>().top_most = tether_links[0];
        segment.GetComponent<EnemyLink>().bottom_most = tether_links[tether_links.Count - 1];

        tether_links[new_link_position - 1].GetComponent<Link>().above = segment;
        tether_links[new_link_position + 1].GetComponent<Link>().below = segment;

        // Recalculate middle
        CalculateMiddleLink();

        InGameUIManager.ui_manager.setMultiplierText();
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

    public void DisableTether()
    {
            tether_links_parent.SetActive(false);
    }

    public void EnableTether()
    {

            HingeJoint2D jo = tether_links[0].GetComponents<HingeJoint2D>()[1];
            jo.autoConfigureConnectedAnchor = false;

            // Calculate how the tether shoould appear between them
            int pos = 0;
            foreach (GameObject obj in tether_links)
            {

                Vector3 dir = beginning_anchor.transform.position - end_anchor.transform.position;
                obj.transform.position = new Vector3(beginning_anchor.transform.position.x, beginning_anchor.transform.position.y, 0) - ((dir / (float)number_of_segments) * pos);
                pos++;
            }

            tether_links_parent.SetActive(true);
    }

    void LateUpdate()
    {
        if (tether_links.Count > 0)
        {

            UpdateTetherGraphics();

        }
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


            /*line.SetVertexCount(tether_links.Count);
            for (int i = 0; i < tether_links.Count; i++)
            {
                line.SetPosition(i, tether_links[i].transform.position);
            }*/
        }

        particle_emitter.particles = particles;
    }
}
