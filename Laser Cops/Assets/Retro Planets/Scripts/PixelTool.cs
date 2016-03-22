using UnityEngine;
using System.Collections;

public class PixelTool : MonoBehaviour {

	public static void DrawLine(Texture2D texture, Color col, Vector2 pos, int length, float angle){
		for(int n = 0; n < length; n++){
			Vector2 point = GetPointOnLine(pos, angle, n);
			texture.SetPixel(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y), col);
		}
	}

	public static void DrawLine(Texture2D texture, Color col, Vector2 start, Vector2 end){
		float dist = Vector2.Distance(start, end);

		for(int n = 0; n <= dist; n++){
			float r = n/dist;
			int x = Mathf.RoundToInt(r * end.x + (1f - r) * start.x);
			int y = Mathf.RoundToInt(r * end.y + (1f - r) * start.y);
			texture.SetPixel(x,y,col);
		}
	}

	public static void DrawVerticalLine(Texture2D texture, Color col, Vector2 start, int height){
		int offsetX = Mathf.RoundToInt(start.x);
		int offsetY = Mathf.RoundToInt(start.y);
		for(int y = 0; y < height; y++){
			texture.SetPixel(offsetX, y+offsetY, col);
		}
	}

	public static void DrawThickVerticalLine(Texture2D texture, Color col, Vector2 start, int height, int width){
		int offsetX = Mathf.RoundToInt(start.x);
		int offsetY = Mathf.RoundToInt(start.y);
		for(int y = 0; y < height; y++){
			for(int x = 0; x < width; x++){
				texture.SetPixel(x+offsetX, y+offsetY, col);
			}
		}
	}

	public static void DrawBresenhamLine(Texture2D texture, Color col, Vector2 start, Vector2 end){
		int dx =  Mathf.RoundToInt(Mathf.Abs(end.x-start.x)); 
		int sx = start.x < end.x ? 1 : -1;
		int dy = -Mathf.RoundToInt(Mathf.Abs(end.y-start.y)); 
		int sy = start.y < end.y ? 1 : -1; 
		int err = dx+dy; 
		int e2 = 0;
		
		for(;;){ 
			texture.SetPixel(Mathf.RoundToInt(start.x), Mathf.RoundToInt(start.y), col);
			if (Mathf.RoundToInt(start.x) == Mathf.RoundToInt(end.x) && Mathf.RoundToInt(start.y) == Mathf.RoundToInt(end.y)) break;
			e2 = 2*err;
			if (e2 >= dy) { err += dy; start.x += sx; } /* e_xy+e_x > 0 */
			if (e2 <= dx) { err += dx; start.y += sy; } /* e_xy+e_y < 0 */
		}
	}

	public static Vector2 GetPointOnLine(Vector2 v1, float angle, int distance) {
		angle *= Mathf.Deg2Rad;
		float x = v1.x + (distance * Mathf.Cos(angle));
		float y = v1.y + (distance * Mathf.Sin(angle));
		return new Vector2(x, y);
	}

	public static void DrawCircle(Texture2D texture, Color col, Vector2 c, int radius){
		int x = radius;
		int y = 0;
		int decisionOver2 = 1 - x;
		int x0 = Mathf.RoundToInt(c.x);
		int y0 = Mathf.RoundToInt(c.y);
		
		while( y <= x ){
			texture.SetPixel(x + x0, y + y0, col);
			texture.SetPixel(y + x0, x + y0, col);
			texture.SetPixel(-x + x0, y + y0, col);
			texture.SetPixel(-y + x0, x + y0, col); 
			texture.SetPixel(-x + x0, -y + y0, col);
			texture.SetPixel(-y + x0, -x + y0, col);
			texture.SetPixel(x + x0, -y + y0, col);
			texture.SetPixel(y + x0, -x + y0, col);
			y++;
			if (decisionOver2<=0){
				decisionOver2 += 2 * y + 1; 
			}
			else{
				x--;
				decisionOver2 += 2 * (y - x) + 1; 
			}
		}
	}

	public static void DrawFilledCircle(Texture2D texture, Color col, Vector2 c, int radius){
		int x = radius;
		int y = 0;
		int decisionOver2 = 1 - x;
		int x0 = Mathf.RoundToInt(c.x);
		int y0 = Mathf.RoundToInt(c.y);
		
		while( y <= x ){
			DrawBresenhamLine(texture, col, new Vector2(x + x0, y + y0), new Vector2(-x + x0, y + y0));
			DrawBresenhamLine(texture, col, new Vector2(y + x0, x + y0), new Vector2(-y + x0, x + y0));
			DrawBresenhamLine(texture, col, new Vector2(-x + x0, -y + y0), new Vector2(x + x0, -y + y0));
			DrawBresenhamLine(texture, col, new Vector2(-y + x0, -x + y0), new Vector2(y + x0, -x + y0));
			y++;
			if (decisionOver2<=0){
				decisionOver2 += 2 * y + 1; 
			}
			else{
				x--;
				decisionOver2 += 2 * (y - x) + 1; 
			}
		}		
	}

	public static void DrawEllipse(Texture2D texture, Color col, int x0, int y0, int x1, int y1) {
		//draws an ellipse inside a rect (xo,yo is bottom left corner, x1, y1 is top rigth corner)
		int a = Mathf.Abs(x1-x0); 
		int b = Mathf.Abs(y1-y0); 
		int b1 = b&1; /* values of diameter */
		long dx = 4*(1-a)*b*b, dy = 4*(b1+1)*a*a; /* error increment */
		long err = dx+dy+b1*a*a, e2; /* error of 1.step */
		
		if (x0 > x1) { x0 = x1; x1 += a; } /* if called with swapped points */
		if (y0 > y1) y0 = y1; /* .. exchange them */
		y0 += (b+1)/2; y1 = y0-b1;   /* starting pixel */
		a *= 8*a; b1 = 8*b*b;
		
		do {
			texture.SetPixel(x1, y0, col); /*   I. Quadrant */
			texture.SetPixel(x0, y0, col); /*  II. Quadrant */
			texture.SetPixel(x0, y1, col); /* III. Quadrant */
			texture.SetPixel(x1, y1, col); /*  IV. Quadrant */
			e2 = 2*err;
			if (e2 <= dy) { y0++; y1--; err += dy += a; }  /* y step */ 
			if (e2 >= dx || 2*err > dy) { x0++; x1--; err += dx += b1; } /* x step */
		} while (x0 <= x1);
		
		while (y0-y1 < b) {  /* too early stop of flat ellipses a=1 */
			texture.SetPixel(x0-1, y0, col); /* -> finish tip of ellipse */
			texture.SetPixel(x1+1, y0++, col); 
			texture.SetPixel(x0-1, y1, col);
			texture.SetPixel(x1+1, y1--, col); 
		}
	}

	public static void DrawFilledEllipse(Texture2D texture, Color col, int x0, int y0, int x1, int y1) {
		//draws an ellipse inside a rect (xo,yo is bottom left corner, x1, y1 is top rigth corner)
		int a = Mathf.Abs(x1-x0); 
		int b = Mathf.Abs(y1-y0); 
		int b1 = b&1; /* values of diameter */
		long dx = 4*(1-a)*b*b, dy = 4*(b1+1)*a*a; /* error increment */
		long err = dx+dy+b1*a*a, e2; /* error of 1.step */
		
		if (x0 > x1) { x0 = x1; x1 += a; } /* if called with swapped points */
		if (y0 > y1) y0 = y1; /* .. exchange them */
		y0 += (b+1)/2; y1 = y0-b1;   /* starting pixel */
		a *= 8*a; b1 = 8*b*b;
		
		do {
			DrawBresenhamLine(texture, col, new Vector2(x0, y0), new Vector2(x1,y0));
			DrawBresenhamLine(texture, col, new Vector2(x0, y1), new Vector2(x1,y1));

			e2 = 2*err;
			if (e2 <= dy) { y0++; y1--; err += dy += a; }  /* y step */ 
			if (e2 >= dx || 2*err > dy) { x0++; x1--; err += dx += b1; } /* x step */
		} while (x0 <= x1);
		
		while (y0-y1 < b) {  /* too early stop of flat ellipses a=1 */
			texture.SetPixel(x0-1, y0, col); /* -> finish tip of ellipse */
			texture.SetPixel(x1+1, y0++, col); 
			texture.SetPixel(x0-1, y1, col);
			texture.SetPixel(x1+1, y1--, col); 
		}
	}

	public static void DrawRing(Texture2D texture, Color col, int x0, int y0, int x1, int y1) {
		int a = Mathf.Abs(x1-x0); 
		int b = Mathf.Abs(y1-y0); 
		int b1 = b&1; /* values of diameter */
		long dx = 4*(1-a)*b*b, dy = 4*(b1+1)*a*a; /* error increment */
		long err = dx+dy+b1*a*a, e2; /* error of 1.step */
		
		if (x0 > x1) { x0 = x1; x1 += a; } /* if called with swapped points */
		if (y0 > y1) y0 = y1; /* .. exchange them */
		y0 += (b+1)/2; y1 = y0-b1;   /* starting pixel */
		a *= 8*a; b1 = 8*b*b;
		
		do {
			if(texture.GetPixel(x1, y0).a == 0f){
				texture.SetPixel(x1, y0, col); /*   I. Quadrant */
			}
			if(texture.GetPixel(x0, y0).a == 0f){
				texture.SetPixel(x0, y0, col); /*  II. Quadrant */
			}
			texture.SetPixel(x0, y1, col); /* III. Quadrant */
			texture.SetPixel(x1, y1, col); /*  IV. Quadrant */
			e2 = 2*err;
			if (e2 <= dy) { y0++; y1--; err += dy += a; }  /* y step */ 
			if (e2 >= dx || 2*err > dy) { x0++; x1--; err += dx += b1; } /* x step */
		} while (x0 <= x1);
		
		while (y0-y1 < b) {  /* too early stop of flat ellipses a=1 */
			texture.SetPixel(x0-1, y0, col); /* -> finish tip of ellipse */
			texture.SetPixel(x1+1, y0++, col); 
			texture.SetPixel(x0-1, y1, col);
			texture.SetPixel(x1+1, y1--, col); 
		}
	}

	public static void DrawOutline(Texture2D texture, Color outlineColor){
		Color32[] pix = texture.GetPixels32();
		//System.Array.Reverse(pix);
		Texture2D tex = new Texture2D(texture.width, texture.height);
		tex.SetPixels32(pix);
		
		for(int y = 0; y < texture.height; y++){
			for(int x = 0; x < texture.width; x++){
				if(tex.GetPixel(x,y).a != 0f){
					if(tex.GetPixel(x+1,y).a == 0f){
						texture.SetPixel(x+1,y, outlineColor);
					}
					if(tex.GetPixel(x-1,y).a == 0f){
						texture.SetPixel(x-1,y, outlineColor);
					}
					if(tex.GetPixel(x,y+1).a == 0f){
						texture.SetPixel(x,y+1, outlineColor);
					}
					if(tex.GetPixel(x,y-1).a == 0f){
						if(y-1 > 0){
							texture.SetPixel(x,y-1, outlineColor);
						}
					}
					
				}
			}
		}
		
	}

	public static void SparseErode(Texture2D texture, int probability, bool applyBox, Vector4 area){
		//x = xmin, y = xmax, z = ymin, w = ymax
		Color32[] pix = texture.GetPixels32();
		Texture2D tex = new Texture2D(texture.width, texture.height);
		tex.SetPixels32(pix);
		
		bool[,] disc = new bool[3, 3]{
			{false, true, false},
			{true, true, true},
			{false, true, false}
		};
		
		for(int y = (int)area.z; y < (int)area.w; y++){
			for(int x = (int)area.x; x < (int)area.y; x++){
				if(tex.GetPixel(x,y).a != 0f){
					bool isEdge = false;
					for(int ky = -1; ky <=1; ky++){
						for(int kx = -1; kx <= 1; kx++){
							if(tex.GetPixel(x+kx,y+ky).a == 0){
								if(applyBox){
									isEdge = true;
								}
								else{
									if(disc[ky+1, kx+1]){
										isEdge = true;
									}
								}
								
							}
						}
					}
					if(isEdge){
						int roll = Random.Range(1,101);
						if(roll < probability){
							texture.SetPixel(x,y, new Color(0f,0f,0f,0f));
						}
					}
				}
				
			}
		}
		
	}

	public static Color Shade(Color col, float percent){
		float factor = 1f-percent;
		Color output = col*factor;
		output.a = 1f;
		return output;
	}

	public static Color Tint(Color col, float percent){
		Color newCol = new Color(1f-col.r, 1f-col.g, 1f-col.b);
		newCol *= percent;
		return col+newCol;
	}

	public static Color GrayIt(Color col){
		return new Color(col.grayscale, col.grayscale, col.grayscale);
	}

	public static Color ColorReplace(Color col, Color replaceColor){
		return GrayIt(col)*replaceColor;
	}

	public static Color NightEffect(Color col){
		return new Color(col.r/4f, col.g/4f, col.b);
	}

	public static Color NightEffect(Color col, float amount){
		return new Color(col.r/amount, col.g/amount, col.b);
	}

	public static Color Blend(Color one, Color two, float transition){
		return Color.Lerp(one,two,transition);
	}

	public static Color RandomColor(){
		return new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f), 1f);
	}

	public static Texture2D ApplyNightEffect(Texture2D texture, float amount){
		Color32[] pix = texture.GetPixels32();
		Texture2D tex = new Texture2D(texture.width, texture.height);
		tex.SetPixels32(pix);
		
		for(int y = 0; y < tex.height; y++){
			for(int x = 0; x < tex.width; x++){
				if(tex.GetPixel(x,y).a != 0f){
					tex.SetPixel(x,y, PixelTool.NightEffect(tex.GetPixel(x,y), amount));
				}
			}
		}
		
		return tex;
	}

	public static Texture2D ApplyColorVariation(Texture2D texture, float variation){
		Color32[] pix = texture.GetPixels32();
		Texture2D tex = new Texture2D(texture.width, texture.height);
		tex.SetPixels32(pix);

		for(int y = 0; y < tex.height; y++){
			for(int x = 0; x < tex.width; x++){
				if(tex.GetPixel(x,y).a != 0f){
					tex.SetPixel(x,y, PixelTool.Shade(tex.GetPixel(x,y), Random.Range(-variation, variation)));
				}
			}
		}

		return tex;
	}

	public static Texture2D SetupTexture(int width, int height){
		Texture2D texture = new Texture2D(width, height);
		texture.filterMode = FilterMode.Point;
		for(int y = 0; y < height; y++){
			for(int x = 0; x < width; x++){
				texture.SetPixel(x,y, new Color(1f,1f,1f,0f));
			}
		}

		return texture;
	}

	public static Texture2D ClampTexture(Texture2D tex){
		int minWidth = tex.width;
		int maxWidth = 0;
		int minHeight = tex.height;
		int maxHeight = 0;
		for(int y = 0; y < tex.height; y++){
			for(int x = 0; x < tex.width; x++){
				if(tex.GetPixel(x,y).a != 0f){
					if(y > maxHeight){
						maxHeight = y;
					}
					if(y < minHeight){
						minHeight = y;
					}
					if(x < minWidth){
						minWidth = x;
					}
					if(x > maxWidth){
						maxWidth = x;
					}
				}
			}
		}

		Texture2D output = new Texture2D(maxWidth-minWidth+1, maxHeight-minHeight+1);
		output.filterMode = FilterMode.Point;
		output.wrapMode = TextureWrapMode.Clamp;

		for(int y = 0; y <= maxHeight-minHeight; y++){
			for(int x = 0; x <= maxWidth-minWidth; x++){
				output.SetPixel(x, y, tex.GetPixel(x+minWidth, y+minHeight));
			}
		}

		return output;
	}

	/*private static string SaveImageName(int width, int height, string goName) {
		return string.Format("{0}/Retro Planets/Sprites/"+goName+"_{1}x{2}_{3}.png",
		                     Application.dataPath, width, height,
		                     System.DateTime.Now.ToString("yyyy-MM-dd_HH.mm-ss"));
	}
	
	public static void Save(Texture2D texture) {
		byte[] bytes = texture.EncodeToPNG();
		string filename = SaveImageName(texture.width, texture.height, texture.name);
		System.IO.File.WriteAllBytes(filename, bytes);
	}*/

}
