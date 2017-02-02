using UnityEngine;
using System.Collections;

public class RandomTimingRayLaserScript : RayLaserScript
{
    public float maxShotDelay = 8f;
    public float minShotDelay = 4f;
    

	void Start () {
        shotCounter = Random.Range(minShotDelay,maxShotDelay) + Time.time;
        layersToIgnore = ~((1 << 12) | (1 << 13) | (1 << 15) | (1 << 0) | (1 << 22) | (1 << 23) | (1 << 24) | (1 << 26) | (1 << 8));

        pink_glow = (Material)Resources.Load("Materials/StreakGlowPink");
        cyan_glow = (Material)Resources.Load("Materials/StreakGlowCyan");
        red_glow = (Material)Resources.Load("Materials/StreakGlowRed");
    }


    void FixedUpdate () {
        if (shotCounter < Time.time)
        {
            laserRenderer.SetWidth(0.5f, 0.5f);
            shooting = true;
            TimeSinceShotCounter = 0;
            laserRenderer.enabled = true;
            shotCounter = Time.time + shotDelay;

            switch (bullet_colour)
            {
                case _Colour.Red:
                    laserRenderer.material = red_glow;
                    break;
                case _Colour.Blue:
                    laserRenderer.material = cyan_glow;
                    break;
                case _Colour.Pink:
                    laserRenderer.material = pink_glow;
                    break;
            }
        }

        if (shooting)
        {
            if (TimeSinceShotCounter > shotDuration)
            {
                SoundMixer.sound_manager.StopBigLazerSound();
                soundStarted = false;
                shooting = false;
                laserRenderer.enabled = false;
            }
            Shoot();
            shotCounter = Random.Range(minShotDelay, maxShotDelay) + Time.time;
        }
    }
}
