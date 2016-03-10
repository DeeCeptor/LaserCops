/*
	This script is placed in public domain. The author takes no responsibility for any possible harm.
	Contributed by Jonathan Czeck
*/
using UnityEngine;
using System.Collections.Generic;

public class TetherLightning : MonoBehaviour
{
    //public Transform target;
    public int zigs = 100;      // How many particles
    public float speed = 1f;    // How quickly do they oscillate
    public float scale = 1f;

    Perlin noise;
    float oneOverZigs;

    private Particle[] particles;
    List<GameObject> links;

    void Start()
    {
        oneOverZigs = 1f / (float)zigs;
        GetComponent<ParticleEmitter>().emit = false;

        GetComponent<ParticleEmitter>().Emit(zigs);
        particles = GetComponent<ParticleEmitter>().particles;
    }

    void Update()
    {
        if (links == null)
            links = Tether.tether.tether_links;

        if (noise == null)
            noise = new Perlin();

        float timex = Time.time * speed * 0.1365143f;
        float timey = Time.time * speed * 1.21688f;
        //float timez = Time.time * speed * 2.5564f;

        for (int i = 0; i < particles.Length; i++)
        {
            float t = ((float)i % (float)links.Count / (float)links.Count);
            int cur_link = (int)(((float)links.Count) * ((float)i / (float)particles.Length));
            int prev_link = Mathf.Max(0, cur_link - 1);//(int)(((float)links.Count) * ((float)Mathf.Max(0, (i - 1)) / (float)particles.Length));

            Vector3 position = Vector3.Lerp(links[prev_link].transform.position,
                links[cur_link].transform.position,
                t);
            //Debug.Log(i + " : " + position + " : between : " + links[prev_link].transform.position + links[cur_link].transform.position + t + " : prev: " + prev_link + " cur: " + cur_link);
            /*
            Vector3 position = Vector3.Lerp(
                links[(int)(((float)links.Count) * ((float)Mathf.Max(0, (i - 1))/ (float)particles.Length))].transform.position, 
                links[(int) (((float) links.Count) * ((float) i / (float) particles.Length))].transform.position, 
                oneOverZigs * (float)i);*/
            //Vector3 position = Vector3.Lerp(transform.position, target.position, oneOverZigs * (float)i);
            Vector3 offset = new Vector3(noise.Noise(timex + position.x, timex + position.y, 0),
                                        noise.Noise(timey + position.x, timey + position.y, 0),
                                        0);
            //position += (offset * scale * ((float)i * oneOverZigs));
            position += (offset * scale * ((float)20 * oneOverZigs));

            particles[i].velocity = (Vector2) particles[i].velocity;
            particles[i].position = position;
            particles[i].energy = 99999f;

            // Oscillate colour
            // Red / white is cool staticy look
            particles[i].color = Color.Lerp(Color.red, Color.white, Random.value);
            //particles[i].color = Color.white;
        }

        GetComponent<ParticleEmitter>().particles = particles;
    }
}