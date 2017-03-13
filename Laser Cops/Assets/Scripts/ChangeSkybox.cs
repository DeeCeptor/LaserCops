using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkybox : MonoBehaviour
{
    public SpriteRenderer fading_blocker;
    public Skybox cur_skybox;

    public List<Material> skyboxes = new List<Material>();


    void Start()
    {
        Next_Skybox();
    }


    public void Next_Skybox()
    {
        if (skyboxes.Count <= 0)
        {
            Debug.Log("Out of skyboxes");
            return;
        }
        
        Fade_New_Skybox(skyboxes[0]);
        skyboxes.RemoveAt(0);
    }

    public void Fade_New_Skybox(Material new_skybox)
    {
        StartCoroutine(fade_new_skybox(1f, new_skybox));
    }
    IEnumerator fade_new_skybox(float time, Material new_skybox)
    {
        while (fading_blocker.color.a < 1)
        {
            Color c = fading_blocker.color;
            c.a += Time.deltaTime * (1f / time);
            fading_blocker.color = c;
            yield return 0;
        }

        // Assign new skybox now that we've completely blocked out that camera
        cur_skybox.material = new_skybox;

        while (fading_blocker.color.a > 0)
        {
            Color c = fading_blocker.color;
            c.a -= Time.deltaTime * (1f / time);
            fading_blocker.color = c;
            yield return 0;
        }
    }
}
