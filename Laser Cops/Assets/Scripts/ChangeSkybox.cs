using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkybox : MonoBehaviour
{
    public SpriteRenderer fading_blocker;
    public Skybox cur_skybox;

    public List<Material> skyboxes = new List<Material>();
    public float time_till_skybox_switch = 3f;

    float timer;


    void Start()
    {
        Next_Skybox();
    }


    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            Next_Skybox();
    }


    public void Next_Skybox()
    {
        timer = time_till_skybox_switch;

        if (skyboxes.Count <= 0)
        {
            Debug.Log("Out of skyboxes");
            return;
        }

        // Get current index
        Material mat = cur_skybox.material;
        int cur_index = skyboxes.FindIndex(u => mat == u) + 1;

        if (cur_index >= skyboxes.Count)
            cur_index = 0;

        Fade_New_Skybox(skyboxes[cur_index]);
    }

    public void Fade_New_Skybox(Material new_skybox)
    {
        StartCoroutine(fade_new_skybox(1f, new_skybox));
    }
    IEnumerator fade_new_skybox(float time, Material new_skybox)
    {
        fading_blocker.gameObject.SetActive(true);
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
