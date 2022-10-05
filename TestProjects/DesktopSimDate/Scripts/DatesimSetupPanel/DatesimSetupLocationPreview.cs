using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupLocationPreview : DatesimSetupDateConnect {
		
		public Sprite[] bgList;
		protected string current = "init";
		public GameObject child;
		public Image img;
		
		protected override void OnEnable( ){
			base.OnEnable();
			if (child == null) child = transform.GetChild(0).gameObject;
			
			if (img == null) img = child.GetComponent<Image>();
			
			SetImage();
		}
		
		protected void SetImage(){
			
			foreach (Sprite t in bgList)
			{
				current = t.name.Replace("_"," ");
				if (current == hub.location)
				{
					img.enabled = true;
					img.sprite = t;
					return;
				}
			}
			img.enabled = false;
		}
		protected override void OnChange()
		{
			if (hub.location == current) return;
				
			else if (hub.location == "none" || hub.location == "") 
			{
				img.enabled = false;
				current = hub.location;
				return;
			}
			else
			{
				SetImage();
			}
		}

	}
}