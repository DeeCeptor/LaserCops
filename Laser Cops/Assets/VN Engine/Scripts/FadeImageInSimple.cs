using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadeImageInSimple : MonoBehaviour 
{
    public float seconds = 1.0f;

    Image i;


    void Start()
    {
        i = this.GetComponent<Image>();
        Color c = i.color;
        c.a = 0;
        i.color = c;
    }


    void Update()
    {
        Color c = i.color;
        c.a += Time.deltaTime / seconds;
        i.color = c;
    }
}
