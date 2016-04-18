using UnityEngine;
using System.Collections.Generic;

public class PlayerDying : MonoBehaviour
{
    Rigidbody2D physics;
    float time_left = 4.0f;
    float rotation_speed = 300.0f;
    float sparks = 0.1f;
    float sparks_cooldown = 0.5f;

    void Start ()
    {
        physics = this.GetComponent<Rigidbody2D>();
        physics.gravityScale = 1.5f;
        time_left = 10f + Random.value * 6.0f;
        rotation_speed *= Mathf.Sign(Random.value - 0.5f);

        GameObject obj = (GameObject) Instantiate((GameObject) Resources.Load("Graphics/Fireball") as GameObject, transform.position, Quaternion.identity);
        obj.transform.parent = this.gameObject.transform;
    }


    void Update ()
    {
        VectorGrid.grid.AddGridForce(this.transform.position, 1, Random.Range(0.1f, 2f), Color.red, true);

        sparks -= Time.deltaTime;
        if (sparks <= 0)
        {
            sparks = sparks_cooldown;
            EffectsManager.effects.ViolentExplosion(this.transform.position);
        }

        // Constantly rotate
        physics.MoveRotation(this.physics.rotation + Time.deltaTime * rotation_speed);

        time_left -= Time.deltaTime;
        if (time_left <= 0)
            Destroy(this.gameObject);
	}


    // SPARKS (we reuse the same spark gameobjects when grinding)
    Dictionary<GameObject, ParticleSystem> in_use_grinding_sparks = new Dictionary<GameObject, ParticleSystem>();
    List<ParticleSystem> free_grinding_sparks = new List<ParticleSystem>();
    public GameObject grinding_sparks;


    // Shower of sparks on a collision!
    void OnCollisionEnter2D(Collision2D collision)
    {
        ParticleSystem sparks;
        // New collision, grab a grinding sparks if we've used one before
        if (free_grinding_sparks.Count > 0)
        {
            sparks = free_grinding_sparks[0];
            free_grinding_sparks.RemoveAt(0);
            sparks.Play();
        }
        else
        {
            // Need to spawn a new grinding sparks
            sparks = (GameObject.Instantiate(Resources.Load("Graphics/Grinding Sparks") as GameObject).GetComponent<ParticleSystem>());
        }

        // Set its position and add it to the dictionary
        sparks.gameObject.transform.position = collision.contacts[0].point;

        if (!in_use_grinding_sparks.ContainsKey(collision.gameObject))
            in_use_grinding_sparks.Add(collision.gameObject, sparks);
    }
    // Show grinding sparks when touching another object
    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject == null)
            return;

        // Update the position of the grinding
        if (in_use_grinding_sparks.ContainsKey(coll.gameObject) && in_use_grinding_sparks[coll.gameObject] != null)
            in_use_grinding_sparks[coll.gameObject].gameObject.transform.position = coll.contacts[0].point;
        else
            in_use_grinding_sparks.Remove(coll.gameObject);
    }
    // Stop grinding against the object we were pushing against
    void OnCollisionExit2D(Collision2D coll)
    {
        if (in_use_grinding_sparks.ContainsKey(coll.gameObject) && in_use_grinding_sparks[coll.gameObject] != null)
        {
            ParticleSystem sparks = in_use_grinding_sparks[coll.gameObject];
            sparks.Stop();
            in_use_grinding_sparks.Remove(sparks.gameObject);
            free_grinding_sparks.Add(sparks);
        }
    }

    public void ClearGrindingSparks()
    {
        // Remove grinding sparks
        List<ParticleSystem> sparks = new List<ParticleSystem>();
        foreach (KeyValuePair<GameObject, ParticleSystem> entry in in_use_grinding_sparks)
        {
            sparks.Add(entry.Value);
        }
        foreach (ParticleSystem ps in free_grinding_sparks)
        {
            sparks.Add(ps);
        }
        for (int x = 0; x < sparks.Count; x++)
        {
            if (sparks != null && sparks[x] != null && sparks[x].gameObject != null)
                Destroy(sparks[x].gameObject);
        }
    }
    void OnDestroy()
    {
        ClearGrindingSparks();
    }
}
