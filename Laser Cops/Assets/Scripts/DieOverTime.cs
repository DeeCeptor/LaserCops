using UnityEngine;
using System.Collections;

//destroy object after a certain number of seconds
public class DieOverTime : MonoBehaviour {

    public float secondsTillDeath = 2f;
    private float deathCounter = 0f;

    //whether the object should fade as it dies
    public bool fade = false;

    //variables for fading
    public bool fading = false;
    public float fadeTime = 2f;
	// Use this for initialization
	void Start () {
        deathCounter = secondsTillDeath + Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(fade && deathCounter - fadeTime< Time.time)
        {
            fading = true;
        }
	    if(deathCounter < Time.time)
        {
          Destroy(gameObject);
        }
        if(fading)
        {
            Fade();
        }
	}

    public void Fade()
    {
        transform.localScale = transform.localScale * 0.99f;
    }

}
