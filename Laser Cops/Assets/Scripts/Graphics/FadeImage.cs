using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeImage : MonoBehaviour
{
    private float alpha = 1;
    public  float fadingOverTime = 0.5f;
    private Color curColor;
    private Image im;

    void Start()
    {
        im = this.GetComponent<Image>();
        curColor = im.color;
    }

    void Update()
    {
        alpha -= Mathf.Clamp01(Time.deltaTime / fadingOverTime);
        curColor.a = alpha;
        im.color = curColor;
    }
}
