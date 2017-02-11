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
    }
    void Start ()
    {
        //do the fireballs so that it won't lag
        Fireworks(transform.position,"red");
        FireballNoGravity(transform.position);
        BurstLargeFireball(transform.position);
        ViolentExplosion(transform.position);

        glow = CameraManager.cam_manager.GetComponent<MKGlow>();

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


    public void GridWake(Vector2 position, float force, float radius, Color color, bool enemy)
    {
        if (GraphicalSettings.graphical_settings.Show_Wakes
            && ((enemy && GraphicalSettings.graphical_settings.Show_Enemy_Wakes) || (!enemy && GraphicalSettings.graphical_settings.Show_Player_Wakes))
            )
            GridExplosion(position, force, radius, color);
    }
    public void GridExplosion(Vector2 position, float force, float radius, Color color)
    {
        if (VectorGrid.grid)
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
        float cur_time = 0;
        glow.GlowIntensity = 0;
        while (glow.GlowIntensity < 1f)
        {
            cur_time += Time.deltaTime;
            glow.GlowIntensity = Mathf.Lerp(0, 1, cur_time / (time / 2));
            yield return null;
        }
        cur_time = 0;
        while (glow.GlowIntensity > 0f)
        {
            cur_time += Time.deltaTime;
            glow.GlowIntensity = Mathf.Lerp(1, 0, cur_time / (time / 2));
            yield return null;
        }
    }

    // Creates a shower of sparks at the designated position
    public void ViolentExplosion(Vector2 position)
    {
        Destroy((GameObject)GameObject.Instantiate(Resources.Load("Graphics/Explosion Sparks") as GameObject,
            position, Quaternion.identity), 2.0f);
    }
    public void FireballNoGravity(Vector2 position)
    {
        Destroy((GameObject)GameObject.Instantiate(Resources.Load("Graphics/Fireball (no gravity)") as GameObject,
            position, Quaternion.identity), 2.0f);
    }
    // Creates a shower of sparks at the designated position
    public void BurstLargeFireball(Vector2 position)
    {
        Destroy((GameObject)GameObject.Instantiate(Resources.Load("Graphics/Burst Large Fireball") as GameObject,
            position, Quaternion.identity), 4.0f);
    }

    public void BulletHitPlayer(Vector2 position)
    {
        Destroy((GameObject)GameObject.Instantiate(Resources.Load("Graphics/Burst Small Fireball") as GameObject,
            position, Quaternion.identity), 2.0f);
    }

    public void TetherGrindSparks(Vector2 position)
    {
        Destroy((GameObject)GameObject.Instantiate(Resources.Load("Graphics/Lasting Grinding Sparks") as GameObject,
            position, Quaternion.identity), 4.0f);
    }
    public void TetherDamageSparks(Vector2 position)
    {
        Destroy((GameObject)GameObject.Instantiate(Resources.Load("Graphics/Tether Damage Sparks") as GameObject,
            position, Quaternion.identity), 4.0f);
    }
    public void ExpandingCircle(Vector2 position, Color c)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Graphics/Expanding Circle") as GameObject, position, Quaternion.identity);
        obj.GetComponent<SpriteRenderer>().color = c;
        Destroy(obj, 3f);
    }
    public GameObject Fireworks(Vector2 position, string color) // Colors are: Red  Green   Blue
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load("Graphics/Fireworks " + color) as GameObject, position, Quaternion.identity);
        Destroy(obj, 4.0f);
        return obj;
    }

    public void SameColorHit(Vector2 position,_Colour bulletColor)
    {
        if (bulletColor == _Colour.Blue)
        {
            Destroy((GameObject)GameObject.Instantiate(Resources.Load("Graphics/Same Color Hit Blue") as GameObject,
                position, Quaternion.identity), 4.0f);
        }
        else
        {
            Destroy((GameObject)GameObject.Instantiate(Resources.Load("Graphics/Same Color Hit Pink") as GameObject,
                position, Quaternion.identity), 4.0f);
        }
    }
    public void BulletReflected(Vector2 position)
    {
        Destroy((GameObject)GameObject.Instantiate(Resources.Load("Graphics/Bullet Reflected") as GameObject,
            position, Quaternion.identity), 3.0f);
    }

    public void PlayersHealed()
    {
        StartCoroutine(ContinualHealingBurst());
    }
    public IEnumerator ContinualHealingBurst()
    {
        foreach (PlayerController p in GameState.game_state.Players)
        {
            TetherLightning.tether_lightning.BurstLightning(p.transform.position, p.transform.position + new Vector3(0, 2), 20, Color.green);
        }
        yield return new WaitForSeconds(0.2f);
        foreach (PlayerController p in GameState.game_state.Players)
        {
            TetherLightning.tether_lightning.BurstLightning(p.transform.position, p.transform.position + new Vector3(0, -2), 20, Color.green);
        }
        yield return new WaitForSeconds(0.3f);
        foreach (PlayerController p in GameState.game_state.Players)
        {
            TetherLightning.tether_lightning.BurstLightning(p.transform.position, p.transform.position + new Vector3(0, 2), 20, Color.green);
        }
        yield return new WaitForSeconds(0.3f);
        foreach (PlayerController p in GameState.game_state.Players)
        {
            TetherLightning.tether_lightning.BurstLightning(p.transform.position, p.transform.position + new Vector3(0, -2), 20, Color.green);
        }
    }


    public GameObject spawnMovingText(Vector3 location, string message)
    {
        GameObject score = Instantiate(Resources.Load("FloatingScore", typeof(GameObject))) as GameObject;
        score.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(-1, 1), 6, 0);
        score.GetComponent<TextMesh>().text = message;
        score.transform.position = location;
        return score;
    }
    public GameObject spawnMovingText(Vector3 location, string message, Vector3 velocity)
    {
        GameObject score = Instantiate(Resources.Load("FloatingScore", typeof(GameObject))) as GameObject;
        score.GetComponent<Rigidbody2D>().velocity = velocity;
        score.GetComponent<TextMesh>().text = message;
        score.transform.position = location;
        return score;
    }
    public GameObject spawnMovingText(Vector3 location, string message, Vector3 velocity, int size)
    {
        GameObject score = Instantiate(Resources.Load("FloatingScore", typeof(GameObject))) as GameObject;
        score.GetComponent<Rigidbody2D>().velocity = velocity;
        score.GetComponent<TextMesh>().text = message;
        score.GetComponent<TextMesh>().fontSize = size;
        score.transform.position = location;
        return score;
    }


    public GameObject[] CutSprite(GameObject obj)
    {
        Sprite corpseSprite = obj.GetComponent<SpriteRenderer>().sprite;
        Texture2D[] corpses = new Texture2D[2];
        Texture2D tex;
        GameObject[] corpse_objects = new GameObject[2];
        
        SpriteRenderer originalRenderer = obj.GetComponent<SpriteRenderer>();

        //instantiate the new sprites for cutting
        for (int i = 0; i < 2; i++)
        {
            GameObject corpseSpawned = (GameObject)Instantiate(Resources.Load("enemies/EmptyCorpse"), obj.transform.position, obj.transform.rotation);
            corpse_objects[i] = corpseSpawned;

            corpseSpawned.transform.localScale = obj.transform.localScale;
            SpriteRenderer newRenderer = corpseSpawned.GetComponent<SpriteRenderer>();

            tex = originalRenderer.sprite.texture;
            corpses[i] = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
            newRenderer.sprite = Sprite.Create(corpses[i], corpseSprite.rect, new Vector2(0.5f, 0.5f));
            newRenderer.color = originalRenderer.color;


            float SpriteSizeRatio = originalRenderer.bounds.size.x / newRenderer.bounds.size.x;
            corpseSpawned.transform.localScale = corpseSpawned.transform.localScale * SpriteSizeRatio;

        }

        //get a random point along a circles edge from the transforms position then get an opposite point 
        Vector2 rand = Random.insideUnitCircle;
        Vector2 point1 = transform.position + (Vector3)rand;
        Vector2 point2 = transform.position - (Vector3)rand;

        Vector2[] pixelLocations = new Vector2[corpses[0].GetPixels32().Length];
        float width = corpses[0].width;

        //random varience to make it look like it was sheared
        //maximum varience
        float xVariance = Random.Range(3f, 5f);
        //current varience
        float xToVary = xVariance;
        //whether x will iterate up or down 
        bool xUp = false;
        float yVariance = Random.Range(3f, 5f);
        float yToVary = yVariance;
        bool yUp = false;
        //counter for when to change x
        int currentIterations = 0;

        int iterationsNeeded = (int)width;
        //set worldspace locations for the pixels
        for (int i = 0; i < pixelLocations.Length; i++)
        {
            pixelLocations[i] = new Vector2((transform.position.x - width / 2f) + (1f * (i % (int)width)) + xToVary, (transform.position.y - width / 2f) + (1f * (i / (int)width)) + yToVary);
            //scripts underneath are to make it jagged
            if (yUp)
            {
                if (yToVary >= yVariance)
                {
                    yUp = false;
                    yToVary = yToVary - 1;
                }
                else
                {
                    yToVary = yToVary + 1;
                }
            }
            else
            {
                if (yToVary <= -yVariance)
                {
                    yUp = true;
                    yToVary = yToVary + 1;
                }
                else
                {
                    yToVary = yToVary - 1;
                }
            }

            if (currentIterations > iterationsNeeded)
            {
                currentIterations = 0;
                if (xUp)
                {
                    if (xToVary >= xVariance)
                    {
                        xUp = false;
                        xToVary = xToVary - 1;
                    }
                    else
                    {
                        xToVary = xToVary + 1;
                    }
                }
                else
                {
                    if (xToVary <= -xVariance)
                    {
                        xUp = true;
                        xToVary = xToVary + 1;
                    }
                    else
                    {
                        xToVary = xToVary - 1;
                    }
                }
            }
            currentIterations = currentIterations + 1;
        }

        //array of pixel arrays
        Color32[][] vertices = new Color32[2][];

        for (int i = 0; i < 2; i++)
        {
            vertices[i] = corpseSprite.texture.GetPixels32();
        }

        for (int i = 0; i < pixelLocations.Length; i++)
        {
            //see which side the point will be on
            if ((pixelLocations[i] - point1).magnitude < (pixelLocations[i] - point2).magnitude)
            {
                //set pixel to clear for the side it's not on
                vertices[1][i] = Color.clear;

            }
            else
            {
                vertices[0][i] = Color.clear;
            }
        }

        for (int i = 0; i < 2; i++)
        {

            corpses[i].SetPixels32(vertices[i]);
            corpses[i].Apply(true);

        }

        // Get original color
        Color c = obj.GetComponent<SpriteRenderer>().color;

        corpse_objects[0].GetComponent<EnemyDying>().JustDied(1);
        corpse_objects[0].GetComponent<SpriteRenderer>().color = c;

        corpse_objects[1].GetComponent<EnemyDying>().JustDied(-1);
        corpse_objects[1].GetComponent<SpriteRenderer>().color = c;

        return corpse_objects;
    }

    void Update ()
    {
	    // Lerp between values
	}
}
