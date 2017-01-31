using UnityEngine;
using System.Collections;

public class EndLevel : MonoBehaviour
{

    private Vector2 playerPos;
    bool activated = false;
    SpriteRenderer sprite;

	void Start ()
    {
        sprite = this.GetComponent<SpriteRenderer>();
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.gameObject.tag.Equals("Player")|| collider.gameObject.tag.Equals("Player")) && !activated)
        {
            Crossed_Finish_Line(collider.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if ((collider.gameObject.tag.Equals("Player") || collider.gameObject.tag.Equals("Player")) && !activated)
        {
            Crossed_Finish_Line(collider.gameObject);
        }
    }


    public void Crossed_Finish_Line (GameObject victorious_player)
    {
        playerPos = victorious_player.transform.position;
        // Spawn party effects for the player!
        EffectsManager.effects.Fireworks(victorious_player.transform.position, "Red").transform.parent = victorious_player.transform;
        EffectsManager.effects.Fireworks(victorious_player.transform.position, "Green").transform.parent = victorious_player.transform;
        EffectsManager.effects.Fireworks(victorious_player.transform.position, "Blue").transform.parent = victorious_player.transform;
        
        // Cut up the finish line
        CutFinishLineSprite();
        Destroy(this.GetComponentInChildren<SpriteRenderer>());

        activated = true;
        GameState.game_state.Victory("", victorious_player);
    }

    public void CutFinishLineSprite()
    {
        Sprite corpseSprite = GetComponentInChildren<SpriteRenderer>().sprite;
        Texture2D[] corpses = new Texture2D[2];
        Texture2D tex;
        GameObject[] corpse_objects = new GameObject[2];

        SpriteRenderer originalRenderer = GetComponentInChildren<SpriteRenderer>();

        float pixelRatio = (Camera.main.orthographicSize * 2) / Camera.main.pixelHeight;

        //instantiate the new sprites for cutting
        for (int i = 0; i < 2; i++)
        {
            GameObject corpseSpawned = (GameObject)Instantiate(Resources.Load("enemies/EmptyCorpse"), transform.position, transform.rotation);
            corpse_objects[i] = corpseSpawned;

            corpseSpawned.transform.localScale = transform.localScale;
            SpriteRenderer newRenderer = corpseSpawned.GetComponent<SpriteRenderer>();

            tex = originalRenderer.sprite.texture;
            corpses[i] = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
            newRenderer.sprite = Sprite.Create(corpses[i], corpseSprite.rect, new Vector2(0.5f, 0.5f));
            newRenderer.color = originalRenderer.color;

            float SpriteSizeRatio = originalRenderer.bounds.size.x / newRenderer.bounds.size.x;
            corpseSpawned.transform.localScale = corpseSpawned.transform.localScale * SpriteSizeRatio;
        }

            Vector2[] pixelLocations = new Vector2[corpses[0].GetPixels32().Length];
            float width = corpses[0].width;

            float xVariance = Random.Range(0.1f, 0.3f);
            //current varience
            float xToVary = xVariance;
            //whether x will iterate up or down 
            bool xUp = false;
            float yVariance = Random.Range(0.1f, 0.3f);
            float yToVary = yVariance;
            bool yUp = false;
            //counter for when to change x
            int currentIterations = 0;

            int iterationsNeeded = (int)width;

            for (int i = 0; i < pixelLocations.Length; i++)
            {
                pixelLocations[i] = new Vector2((transform.position.x - width / 2f) + (pixelRatio * (i % (int)width)) + xToVary, (transform.position.y - width / 2f) + (pixelRatio * (i / (int)width)) + yToVary);
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

        Color32[][] vertices = new Color32[2][];

        for (int i = 0; i < 2; i++)
        {
            vertices[i] = corpseSprite.texture.GetPixels32();
        }

        for (int i = 0; i < pixelLocations.Length; i++)
        {
            //see which side the point will be on
            if (pixelLocations[i].y > playerPos.y)
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

        Color c = GetComponentInChildren<SpriteRenderer>().color;

        corpse_objects[0].GetComponent<EnemyDying>().JustDied(1);
        corpse_objects[0].GetComponentInChildren<SpriteRenderer>().color = c;

        corpse_objects[1].GetComponent<EnemyDying>().JustDied(-1);
        corpse_objects[1].GetComponentInChildren<SpriteRenderer>().color = c;
    }

    void Update ()
    {
	}
}
