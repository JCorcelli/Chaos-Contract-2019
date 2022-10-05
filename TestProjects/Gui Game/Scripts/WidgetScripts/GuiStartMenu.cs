
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


	
	public class GuiStartMenu : SelectAbstract {
		
		
		protected GuiDict dict;
		//protected GuiDict activator;
		
		protected override void OnEnable( ){
			base.OnEnable();
			Connect();
		}
		protected virtual void Connect() {
			if (dict != null) return;
			dict = GetComponent<GuiDictComponent>().dict;
			
			
			// debugging the acii def
			//foreach (string s in dict[WINDOW_ASCII].Keys)
			//{
			//	foreach (string s2 in dict[WINDOW_ASCII][s].Keys)
			//	Debug.Log(s+"..."+s2);
			//}
			
				
			//Debug.Log(dict.GetPath().ToString());
			
			dict.onUpdate += OnMessage;
			
			if (dict["active"] == null)
				dict.Add("active", "1");
			
			dict = dict["active"][0];
			
			
			dict.onUpdate += OnMessage;
			Sync();
			
		}
		
		public virtual void Sync() {
			
			if (dict.key == "1")
				gameObject.SetActive(true);
			else
				gameObject.SetActive(false);
		}
		public virtual void OnMessage(GuiDict updateDict) {
			// assume any child hit will activate, except special menu items won't
			
			if (updateDict != dict) //&& !exemptlist.Contains(updateDict.key))
				dict.key = "0";
			Sync();
		}
		protected override void OnClick() {
			
		}
		
	}
	
}