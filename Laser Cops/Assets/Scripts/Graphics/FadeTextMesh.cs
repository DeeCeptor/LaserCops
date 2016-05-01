using UnityEngine;
using System.Collections;

public class FadeTextMesh : MonoBehaviour
{
    private float alpha = 1;
    private float fadingOverTime = 1f;
    private Color curColor;

    void Start()
    {
        this.GetComponent<MeshRenderer>().sortingLayerName = "Text";
        curColor = transform.GetComponent<TextMesh>().color;
    }

    void Update()
    {
        alpha -= Mathf.Clamp01(Time.deltaTime / fadingOverTime);
        curColor.a = alpha;
        transform.GetComponent<TextMesh>().color = curColor;

        if (curColor.a <= 0)
            Destroy(this.gameObject);
    }
}
