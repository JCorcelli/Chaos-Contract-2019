
using UnityEngine;
using UnityEngine.Assertions;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MEC;

using Utility.GUI;
using SelectionSystem;
using System.Text;
using static GuiGame.GuiGameVars;

namespace GuiGame
{


	
	public class GuiStartButton : SelectAbstract {
		
		
		protected GuiDict dict;
		protected GameObject comp;
		
		protected override void OnEnable( ){
			base.OnEnable();
			Connect();
		}
		protected virtual void Connect() {
			if (dict != null) return;
			dict = GetComponent<GuiDictComponent>().dict.FindAll("StartMenu", true);
			
			// apparently it's name/type

			if (dict.Count > 0)
				dict = dict[dict.Count - 1];
			
			if (dict["active"] == null)
				dict.Add("active", "0");
			
			dict = dict["active"][0];
			
			//Debug.Log(dict.GetPath().ToString());


			
			dict.onUpdate += OnMessage;
			

			/*
			BText s = dict.GetComponentPath();
			
			// search
			
			Transform t = transform;
			int pos = s.IndexOf(t.name); // pretty big assumption
			
			if (pos >= 0)
			{
				char c = ' ';
				while (pos < s.Length && c!= '/')
					c = s[pos++];
			}
			string n = s.ToString();
			
			t = transform.Find("StartMenu");
			
			
			comp = t.gameObject;
			//	comp.SetActive(false);
			*/
		}
		public virtual void OnMessage(GuiDict updateDict) {
			//dict.Remove("active");
			//comp.SetActive(false);

		}
		protected override void OnClick() {
			
			transform.SetSiblingIndex(transform.parent.childCount - 1);
			
			if (dict.key == "1")
				dict.key = "0";
			else
				dict.key = "1";
			
			dict.SendUpdate();
			
		}
	}
	
}