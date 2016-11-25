using UnityEngine;
using System.Collections;

public class ExplodeOnTime : MonoBehaviour {

    public float TimeTillDetonation = 8f;
    public float DetonationCounter = 0f;
    public TextMesh countdownTimer;
    public bool active = false;

    public GameObject bullet;
    public float numberOfBullets = 20;

    // Use this for initialization
    void Start () {
	
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("MainCamera"))
        {
            DetonationCounter = Time.time + TimeTillDetonation;
            active = true;
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (!active)
        {
            DetonationCounter = Time.time + TimeTillDetonation;
            active = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (active)
        {
            countdownTimer.text = ((int)(DetonationCounter - Time.time)).ToString();
            if (DetonationCounter < Time.time)
            {
                Explode();
            }
        }
    }

    public void Explode()
    {
        float angle = 0f;
        float anglePerIteration = 360 / numberOfBullets;
        Vector3 targetDirection;
        for (int i = 0; i < numberOfBullets;i++)
        {
            angle = anglePerIteration * i;
            targetDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle),0);
            targetDirection = transform.TransformDirection(targetDirection);
            GameObject bulletSpawned = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
            BulletScript bulletStats = bulletSpawned.GetComponent<BulletScript>();
            bulletStats.target = transform.position + targetDirection;
        }
        EffectsManager.effects.ViolentExplosion(transform.position);
        EffectsManager.effects.TetherGrindSparks(transform.position);
        EffectsManager.effects.BurstLargeFireball(transform.position);
        SoundMixer.sound_manager.PlayGettingHitExplosion();
        Destroy(gameObject);
    }
}
