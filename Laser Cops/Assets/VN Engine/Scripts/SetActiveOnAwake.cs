using UnityEngine;
using System.Collections;

// Enables a game object upon awakening, then destroys itself
public class SetActiveOnAwake : MonoBehaviour 
{
    public GameObject object_to_activate;
    public bool destroy_this_obj = true;

    void Awake ()
    {
        object_to_activate.SetActive(true);

        if (destroy_this_obj)
            Destroy(this.gameObject);
        else
            Destroy(this);
    }
}
