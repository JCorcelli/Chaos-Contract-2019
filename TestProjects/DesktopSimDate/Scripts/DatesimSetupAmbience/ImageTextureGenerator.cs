using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Utility.UI
{
	public class ImageTextureGenerator : MonoBehaviour {

		public RawImage image ;
		public RectTransform rectTransform;
		
		public static Texture2D tex;
		public static Sprite sprite;
		public static Color[] fillPixels;
		public int texSize = 512;
		
		protected void MakeTex(){
		tex = new Texture2D (512, 512, TextureFormat.ARGB32, false);
		tex.name = name;
		Color fillColor = Color.clear;
		fillPixels = new Color[tex.width * tex.height];
		
		for (int i = 0; i < fillPixels.Length; i++)
		{
			fillPixels[i] = fillColor;
		}

		tex.SetPixels(fillPixels);


		tex.Apply();

		}
		
		public static void SetSubsurface(Texture2D source, Vector2 offset, bool center = true){
			// the source is a recttransform with an image
			Color[] setPixels = source.GetPixels();
			
			// really it should be scaled like...
			//w = sourceRect.width
			//h = sourceRect.height
			// it is printing to this recttransform
			
			if (center) {
				offset.x = offset.x - source.width / 2;
				offset.y = offset.y - source.height / 2;
			}
			
			int startx = (int)offset.x;
			if (startx < 0) startx= 0;
			int starty = (int)offset.y;
			if (starty < 0) starty= 0;
			
			int endx = startx + tex.width;
			if (endx >= tex.width) endx = tex.width ;
			int endy = starty+ tex.height;
			if (endy >= tex.height) endy = tex.height ;
			
			
			// I haven't calculated skipped pixels so this could be an issue
			
			// skip columns
			int pos;
			int posy;
			int i = 0;
			for (int y = starty; y < endy; y ++)
			{
				posy = y * tex.width;
				// draw rows
				for (int x = startx; x < endx; x ++)
				{
					// position is cursor position in a huge image
					pos = posy + x;
					fillPixels[pos] = setPixels[i];
					x ++;
					i ++;
				}
				y ++;
			}
				
				
			tex.SetPixels(fillPixels);


			tex.Apply();
			
		}
		
		
		protected void Awake(){
			
			if (image == null) image =GetComponent<RawImage>();
			if (rectTransform == null) rectTransform =GetComponent<RectTransform>();
			
			if (tex == null) MakeTex();
			image.texture = tex;
			
		}
	}

}