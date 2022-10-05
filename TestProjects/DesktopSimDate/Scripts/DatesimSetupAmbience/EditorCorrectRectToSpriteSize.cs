using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Utility.UI
{
	[ExecuteInEditMode]
	public class EditorCorrectRectToSpriteSize : MonoBehaviour {

		protected Image image;
		public bool run = false;
		public RectTransform rt;
		protected void Update(){
			if (!run) 
			{
				run = !(enabled = false);
				return;
			}
			if (image == null) image = GetComponent<Image>();
			float realWidth = image.sprite.rect.width;
			float realHeight = image.sprite.rect.height;		
			if (rt == null) rt = transform as RectTransform;
				
			rt.sizeDelta = new Vector2(realWidth, realHeight);
		}
		
	}

}