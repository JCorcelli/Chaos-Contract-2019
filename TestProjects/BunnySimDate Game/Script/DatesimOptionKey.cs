using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimOptionKey : AbstractKeyHandler {
		// This is intended for big applications that need to coordinate actions
		
		
		
		
		public bool bool_value = true;
		public int message = 0;
		public DatesimHub.EffectEnum[] effects = new DatesimHub.EffectEnum[]{};
		
		
		public ConnectHubDelegate onConnect;
		
		
		
		public DatesimVariables vars;
		
		protected override void OnEnable( ){
			base.OnEnable();
			
			if (vars == null) vars = GetComponentInParent<DatesimVariables>();
			
			if (vars == null) {
				Debug.Log("no vars, this broke", gameObject);
				return; 
			}
			vars.onConnect -= OnConnect;
			vars.onConnect += OnConnect;
			//vars.onChange += OnChange;
			
			if (colorOb == null)
				AddRecolorOb();
			
			if (gameObject.GetComponent<DatesimOptionSelect>() == null)
				AddSelectOb();
			
			
			if (gameObject.GetComponent<DatesimWidgetEffect>() == null)
				AddEffectOb();
		}
		
		
		protected virtual void AddEffectOb() {
			if (gameObject.GetComponent<DatesimWidgetEffect>() == null)
			{
				
				DatesimWidgetEffect newOb = gameObject.AddComponent(typeof(DatesimWidgetEffect)) as DatesimWidgetEffect;
				
				newOb.message = message;
				
				if (effects.Length > 0) 
				{
					newOb.effects = effects;
				}
				
				
			}
		}
		
		protected virtual void AddSelectOb() {
			DatesimOptionSelect newOb = gameObject.AddComponent(typeof(DatesimOptionSelect)) as DatesimOptionSelect;
			newOb.onClick += OnClick;
		}
		
		DatesimOptionRecolor colorOb;
		protected virtual void AddRecolorOb() {
			colorOb = gameObject.AddComponent(typeof(DatesimOptionRecolor)) as DatesimOptionRecolor;
			colorOb.message = message;
		}
		
		
		
		protected virtual void Connect() {
			
			vars.Connect(this);
			
			
			// everything looking for this, in scope, will now have it
			
		}
		protected override void OnDisable( ){
			base.OnDisable();
			if (vars == null) return;
			
			vars.onConnect -= OnConnect;
			
			// something can check if this is disabled.
		}
		
		
		
		
		protected virtual void OnConnect(Object other) {
			// there's no telling which will connect first. Only one needs to use the connection, whether it's here or OnEnable.
			
			// if (thisenum == (int)Enumerator) 
			// connected = other;
			// hub.connected = gameObject;
			
			// use the other gameObject
			// if onconnect != null onConnect() something else grabs the connection, done
		}
		public int availableStage = 1;
		protected void OnClick() {OnPress();}
		protected override void OnPress() {
			base.OnPress();
			if (availableStage != vars.stage) return;
			
			
			// else I want all the things I generated to do their stuff
			vars.option = message;
			//colorOb.AddColor();
			vars.Response();
			vars.option = -1;
			
		}
		
		

	}
}