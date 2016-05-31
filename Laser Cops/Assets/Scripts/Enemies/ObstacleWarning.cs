using UnityEngine;
using System.Collections;

public class ObstacleWarning : MonoBehaviour 
{
    public bool play_obstacle_warning_sound = true;
    public GameObject warning_obj;

	void Start () 
	{
	
	}
	

	void Update () 
	{
        CheckActive();
    }
    public void CheckActive()
    {
        if (GetComponent<SpriteRenderer>().IsVisibleFrom(Camera.main))
        {
            StartWarning();
        }
    }

    public void StartWarning()
    {

        if (play_obstacle_warning_sound)
        {
            SoundMixer.sound_manager.PlayObstacleWarning();
        }
        // Enable other object
        warning_obj.SetActive(true);

        Destroy(this.gameObject);
    }
}
