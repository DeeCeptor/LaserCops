using UnityEngine;
using System.Collections;

// Use this class to put explosions and generic stuff
public class EffectsManager : MonoBehaviour
{
    public static EffectsManager effects;


    void Awake ()
    {
        effects = this;
    }
    void Start ()
    {
	
	}



    // Creates a shower of sparks at the designated position
    public void ViolentExplosion(Vector2 position)
    {
        Destroy((GameObject)GameObject.Instantiate(Resources.Load("Graphics/Explosion Sparks") as GameObject,
            position, Quaternion.identity), 2.0f);
    }


    void Update ()
    {
	
	}
}
