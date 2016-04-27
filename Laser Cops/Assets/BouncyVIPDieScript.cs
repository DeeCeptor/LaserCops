using UnityEngine;
using System.Collections;

public class BouncyVIPDieScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == LayerMask.NameToLayer("Death Zone"))
        {
            Die();
        }
    }

    public void CutSprite()
    {
        Sprite corpseSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        Texture2D[] corpses = new Texture2D[2];
        Texture2D tex;

        //instantiate the new sprites for cutting
        for (int i = 0; i < 2; i++)
        {
            GameObject corpseSpawned = (GameObject)Instantiate(Resources.Load("enemies/EmptyCorpse"), transform.position, transform.rotation);
            corpseSpawned.transform.localScale = transform.localScale;

            tex = gameObject.GetComponent<SpriteRenderer>().sprite.texture;
            corpses[i] = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
            corpseSpawned.GetComponent<SpriteRenderer>().sprite = Sprite.Create(corpses[i], corpseSprite.rect, new Vector2(0.5f, 0.5f));
            corpseSpawned.GetComponent<SpriteRenderer>().color = gameObject.GetComponent<SpriteRenderer>().color;
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

        Texture2D sprite = corpses[0];
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


    }

    public void Die()
    {
        SoundMixer.sound_manager.Play8bitExplosion();
        EffectsManager.effects.ViolentExplosion(this.transform.position);
        CutSprite();
        Destroy(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
