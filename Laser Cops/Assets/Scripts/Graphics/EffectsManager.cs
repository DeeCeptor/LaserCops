using UnityEngine;
using System.Collections;
using MKGlowSystem;

// Use this class to put explosions and generic stuff
public class EffectsManager : MonoBehaviour
{
    public static EffectsManager effects;

    // Glow variables
    public MKGlow glow;
    public float from_blur_spread;  // width of glow
    public float normal_blur_spread;    
    public float from_blur_iterations;      // number of blurs
    public float normal_blur_iterations;
    public float from_blur_offset;      // distance of blurs
    public float normal_blur_offset;
    public float from_blur_samples;     // more blurring
    public float normal_blur_samples;
    public float from_glow_intensity;   // more glowing
    public float normal_glow_intensity;
    public float speed_of_adjusting_glow = 1.0f;

    void Awake ()
    {
        effects = this;
        glow = Camera.main.GetComponent<MKGlow>();

        normal_blur_iterations = glow.BlurIterations;
        normal_blur_offset = glow.BlurOffset;
        normal_blur_samples = glow.Samples;
        normal_blur_spread = glow.BlurSpread;
        normal_glow_intensity = glow.GlowIntensity;
        from_blur_iterations = glow.BlurIterations;
        from_blur_offset = glow.BlurOffset;
        from_blur_samples = glow.Samples;
        from_blur_spread = glow.BlurSpread;
        from_glow_intensity = glow.GlowIntensity;
    }
    void Start ()
    {
        //FlashScreen();
    }


    public void GridExplosion(Vector2 position, float force, float radius, Color color)
    {
        VectorGrid.grid.AddGridForce((Vector3)position, force, radius, color, true);
    }
    
    public void FlashScreen()
    {
        // Use blur iterations
        from_blur_iterations = 11;
        StartCoroutine(FlashScreenBriefly(2.0f));
    }
    public IEnumerator FlashScreenBriefly(float time)
    {
        float cur_blur_iterations = glow.BlurIterations;
        float cur_time = 0;
        float wait_time = time / 10;
        while (glow.BlurIterations < from_blur_iterations)
        {
            cur_time += wait_time;
            glow.BlurIterations = (int) Mathf.Lerp(cur_blur_iterations, from_blur_iterations, cur_time / (time / 2));
            yield return new WaitForSeconds(wait_time);
        }
        cur_blur_iterations = glow.BlurIterations;
        cur_time = 0;
        while (glow.BlurIterations > normal_blur_iterations)
        {
            cur_time += wait_time;
            glow.BlurIterations = (int) Mathf.Lerp(cur_blur_iterations, normal_blur_iterations, cur_time / (time / 2));
            yield return new WaitForSeconds(wait_time);
        }
        Debug.Log("D0ne");
    }

    // Creates a shower of sparks at the designated position
    public void ViolentExplosion(Vector2 position)
    {
        Destroy((GameObject)GameObject.Instantiate(Resources.Load("Graphics/Explosion Sparks") as GameObject,
            position, Quaternion.identity), 2.0f);
    }


    public void BulletHitPlayer(Vector2 position)
    {
        Destroy((GameObject)GameObject.Instantiate(Resources.Load("Graphics/Burst Small Fireball") as GameObject,
            position, Quaternion.identity), 2.0f);
    }


    public void TetherDamageSparks(Vector2 position)
    {
        Destroy((GameObject)GameObject.Instantiate(Resources.Load("Graphics/Tether Damage Sparks") as GameObject,
            position, Quaternion.identity), 4.0f);
    }


    void Update ()
    {
	    // Lerp between values
	}
}
