using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeTextInAndOut : MonoBehaviour
{
    public static FadeTextInAndOut music_info;

    private float alpha = 1;
    public float fadingOverTime = 1f;
    private Color curColor;
    private Text im;
    bool working = false;
    bool fading_in = false;
    float normal_wait_time = 4f;
    float waiting_time = 3f;

    void Awake ()
    {
        music_info = this;
    }


    public void Start_Fading_In_Then_Out()
    {
        if (working)
            return;

        im = this.GetComponent<Text>();
        curColor = im.color;

        working = true;
        fading_in = true;
        waiting_time = normal_wait_time;
        alpha = 0;
        curColor.a = 0;
        im.color = curColor;
    }


    void Update()
    {
        if (working)
        {
            // Fade in
            if (fading_in)
            {
                alpha += Mathf.Clamp01(Time.deltaTime / fadingOverTime);
                curColor.a = alpha;
                im.color = curColor;

                if (alpha >= 1)
                {
                    fading_in = false;
                    waiting_time = 3f;
                }
            }
            // Wait a bit
            else if (waiting_time >= 0)
            {
                waiting_time -= Time.deltaTime;
            }
            // Fade out
            else
            {
                alpha -= Mathf.Clamp01(Time.deltaTime / fadingOverTime);
                curColor.a = alpha;
                im.color = curColor;

                if (alpha <= 0)
                    working = false;

            }
        }
    }
}
