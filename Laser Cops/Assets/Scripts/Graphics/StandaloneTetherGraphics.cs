using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandaloneTetherGraphics : MonoBehaviour
{
    // TETHER GRAPHICS
    public int zigs = 300;      // How many particles
    public float speed = 1f;    // How quickly do they oscillate
    public float scale = 1.5f;

    public Color primary_colour = Color.green;
    public Color secondary_colour = Color.white;

    public Transform[] curve_points = new Transform[3];

    Perlin noise;
    float oneOverZigs;

    private Particle[] particles;

    public ParticleEmitter particle_emitter;


    void Start ()
    {
        oneOverZigs = 1f / (float)zigs;
        particle_emitter.emit = false;

        particle_emitter.Emit(zigs);
        particles = particle_emitter.particles;
    }


    public void Update()
    {
        if (noise == null)
            noise = new Perlin();

        float timex = Time.time * speed * 0.1365143f;
        float timey = Time.time * speed * 1.21688f;

        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].velocity = (Vector2)particles[i].velocity;
            particles[i].energy = 99999f;

            // Interpolate the position of this particle
            Vector2 pos = Bezier2(curve_points[0].position, curve_points[1].position, curve_points[2].position, (float) i / (float) particles.Length);
            //Debug.Log(pos);
            particles[i].position = pos;

            // Oscillate colour
            // Red / white is cool staticy look
            Color c = Color.Lerp(primary_colour, secondary_colour, Random.value);
            c.a = Random.value;
            particles[i].color = c;
        }

        particle_emitter.particles = particles;
    }


    public Vector2 Bezier2(Vector2 Start, Vector2 Control, Vector2 End, float t)
    {
        return (((1-t)*(1-t)) * Start) + (2 * t* (1 - t) * Control) + ((t* t) * End);
    }
}
