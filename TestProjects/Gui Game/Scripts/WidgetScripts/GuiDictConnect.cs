
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


	
	public abstract class GuiDictConnect : UpdateBehaviour {
		
		protected GuiDict dict;
		protected override void OnEnable( ){
			base.OnEnable();
			Connect();
		}
		public virtual void Connect() {
			if (dict != null) return;
			
			dict = GetComponent<GuiDictComponent>().dict;
			dict.onUpdate += OnMessage;
		}
		public virtual void OnMessage(GuiDict updateDict) {
			Debug.Log("update = " + updateDict.key);
		}
	}
	
}