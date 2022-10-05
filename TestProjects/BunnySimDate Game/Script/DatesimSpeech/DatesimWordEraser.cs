using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SelectionSystem;

namespace Datesim
{
	
	public class DatesimWordEraser : SelectAbstract {

		
		protected Text text;
		//new public Transform rectTransform;
		public RectTransform highlight ;
		public RectTransform rectTransform;
		protected override void OnEnable () { 
			base.OnEnable();
			text = GetComponent<Text>();
		}
		protected override void OnClick( ) {
			text.text = "";
			//text.SetLayoutDirty(); // this shrinks it, to delete it I think I have to disable
		
		}
		
	}
}