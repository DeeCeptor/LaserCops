using UnityEngine;
using System.Collections;

public class RetroPlanet : PixelAsset {

	public int radius = 32;
	public Color highlightColor = Color.cyan;
	public Color shadeColor = Color.black;
	[Range(0f, 2.01f)]
	public float shadeWidth = 0.7f;

	protected override void SetupTexture ()
    {
		texDimensions = (radius*2)+10;
		base.SetupTexture ();
	}

	public override void Draw ()
    {
		int centerX = texture.width/2;
		int centerY = texture.height/2;

		PixelTool.DrawFilledCircle(texture, shadeColor, new Vector2(centerX, centerY), radius);

		for (int y = -radius; y <= radius; y++)
        {
			int x1 = (int)Mathf.Sqrt(radius * radius - y * y);
			for (int x = -x1; x <= x1; x++)
            {
				float n = Random.Range(0, x1) * (2.01f-shadeWidth);
				if (n > x1 + x)
                {
					texture.SetPixel(x+centerX, y+centerY, highlightColor);
				}
			}
		}
	}

    /*
    float start_time;
    float prev_time;
    float cur_time;
    void Update()
    {
        cur_time += Time.deltaTime;

        if (cur_time >= prev_time + 1.0f)
        {
            prev_time = Time.time;
            shadeWidth = Mathf.Lerp(0, 2.01f, (cur_time - start_time) / 3f);
            GenerateAsset();
        }
    }*/
}
