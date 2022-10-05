using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Utility.UI
{
	public class ImageBrushAnimation : UpdateBehaviour {
		// just destroy it and image stays the same
		protected float count = 1f;
		public float loopTime = 1f;
		protected int current = 0;
		public Sprite[] list;
		
		protected Image image;
		
		protected override void OnEnable(){
			base.OnEnable();
			count = loopTime;
			if (image == null)
			image = GetComponent<Image>();
			image.sprite = list[current];
			if (image == null) Debug.Log("no image", gameObject);
		}
		protected void CorrectSize(){
			// I want to use a size of canvas and sprite size to fix the issues
		}
		protected override void OnUpdate(){
			base.OnUpdate();
			count -= Time.deltaTime;
			if (count > 0) return;
			count += loopTime;
			current = (int)Mathf.Repeat(current + 1, list.Length);
			image.sprite = list[current];
		}	
		
	}

}