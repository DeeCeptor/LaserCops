using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WaitThenFadeAndLoad : MonoBehaviour 
{
    public float seconds_to_wait = 4.0f;
    public string scene_to_load;

    bool loading_scene = false;
    Image img;

	void Start () 
	{
        img = this.GetComponent<Image>();
	}
	

	void Update () 
	{
        seconds_to_wait -= Time.deltaTime;

        if (seconds_to_wait < 0)
        {
            // Start fading in
            Color c = img.color;
            c.a += Time.deltaTime;
            img.color = c;

            if (c.a >= 1 && !loading_scene)
                LoadScene();
        }
	}


    void LoadScene()
    {
        loading_scene = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene_to_load);
    }
}
