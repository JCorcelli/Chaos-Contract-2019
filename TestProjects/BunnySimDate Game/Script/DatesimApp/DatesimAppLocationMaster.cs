using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAppLocationMaster : DatesimAppConnect {
		
		public Sprite[] bgList;
		protected string current = "init";
		public GameObject child;
		public Image img;
		
		protected override void OnEnable( ){
			base.OnEnable();
			if (child == null) child = transform.GetChild(0).gameObject;
			
			if (img == null) img = child.GetComponent<Image>();
			
			if (vars.locationOptions.Length < 1)
				SetVars();
			SetImage();
		}
		protected void SetVars(){
			
			string[] newArray = new string[bgList.Length];
			
			for (int i = 0; i < bgList.Length ; i ++)
			{
				newArray[i] = bgList[i].name.Replace("_"," ");
				
			}
			
			vars.locationOptions = newArray;
			//Connect();
		}
		protected void SetImage(){
			
			foreach (Sprite t in bgList)
			{
				current = t.name.Replace("_"," ");
				if (current == vars.location)
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
			if (vars.location == current) return;
				
			else if (vars.location == "none" || vars.location == "") 
			{
				img.enabled = false;
				current = vars.location;
				return;
			}
			else
			{
				SetImage();
			}
		}

	}
}