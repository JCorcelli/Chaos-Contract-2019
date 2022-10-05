using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SelectionSystem;

namespace Datesim
{
	
	public class DatesimWordInterpreter : SelectAbstract {

		
		public Text text;
		public Image img;
		public Sprite thisSprite;
		//new public Transform rectTransform;
		public RectTransform highlight ;
		public RectTransform rectTransform;
		protected override void OnEnable () { 
			base.OnEnable();
			text = GetComponent<Text>();
		}
		protected override void OnClick( ) {
			if (img != null) return;
			DestroyImmediate(text);
			img = gameObject.AddComponent<Image>();
			img.sprite = thisSprite;
		}
		
	}
}