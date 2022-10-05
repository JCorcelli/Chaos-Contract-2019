using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace Datesim.Setup
{
	public class DatesimAmbienceRectUtil : UpdateBehaviour {
		// so with this, on enable it keeps an image at the same size. It assumes the pivot is centered. It can be scaled with scaleOFfset.
		
		public RectTransform rectTransform;
		public Image image;
		protected Vector3 position; // as it says
		protected float canvasWidth = 1200f;
		public float scaleOffset = 1f;
		
		protected float correction = 0f;
		protected float scale = 0f;
		protected Sprite sprite;

		protected override void OnEnable(){
			base.OnEnable();
			
			if (image == null)
			image = GetComponent<Image>();
			if (image == null)
				Debug.Log("error, no image", gameObject);
			if (rectTransform == null)
			rectTransform = GetComponent<RectTransform>();
		
			// this will assume the first place it's enabled is correct
		}
		
		public bool positioned = false;
		public void SetPosition(Vector3 newpos){
			
			if (rectTransform == null)
			rectTransform = GetComponent<RectTransform>();
			rectTransform.position = newpos;
			CheckParent();
			position = rectTransform.localPosition / scale;
			positioned = true;
		}
		
		public void SetScaleOffset(float newScale){
			scaleOffset = newScale;
			Rescale();
		}
		
		
		

		public DatesimAmbienceRectUtil CloneTo(Transform nt){
			DatesimAmbienceRectUtil cl = Clone();
			cl.SetParent(nt);
			return cl;
		}
		public DatesimAmbienceRectUtil Clone(){
			DatesimAmbienceRectUtil cl = GameObject.Instantiate(this) as DatesimAmbienceRectUtil;
			
			cl.GetComponent<RectTransform>().SetParent(this.rectTransform.parent, false);
			cl.SetPosition ( this.rectTransform.position );
			return cl;
		}
		public void CheckParent(){
			
			RectTransform rt = rectTransform.parent as RectTransform;
			
			correction = rt.rect.width;
			
			scale = correction / 1200f;
			
			
		}
		public void Rescale(){
			
			CheckParent();
			if (image == null)
			image = GetComponent<Image>();
			sprite = image.sprite;
			
			rectTransform.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height) * scale * scaleOffset;
			
		}
		public void Reposition(){
			if (!positioned) return;
			CheckParent();
			
			rectTransform.localPosition = position * scale;
			
			
		}
		
		public void SetParent(Transform nt){
			rectTransform.SetParent(nt, false);
			Fix();
		}
		public void Fix(){
			Rescale(); // sprite and offset
			Reposition(); // basic. needs SetPosition called
			
			
		}	
		
	}

}