
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


	
	public class GuiCalculator : SelectAbstract {
		
		
		protected GuiDict dict;
		//protected GuiDict activator;
		
		protected override void OnEnable( ){
			base.OnEnable();
			Connect();
		}
		protected virtual void Connect() {
			if (dict != null) return;
			dict = GetComponent<GuiDictComponent>().dict;
			
			//Debug.Log(dict.GetPath().ToString());
			
			// need to reparent the dictionary after duplicating it
		}
		
		public virtual void Sync() {
			
			if (dict.key == "1")
				gameObject.SetActive(true);
			else
				gameObject.SetActive(false);
		}
		public virtual void OnMessage(GuiDict updateDict) {
		}
		protected override void OnClick() {
			// start app
			dict.SendUpdate();
		}
		
	}
	
}