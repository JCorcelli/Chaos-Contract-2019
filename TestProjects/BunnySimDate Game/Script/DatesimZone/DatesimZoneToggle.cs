using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimZoneToggle : AbstractButtonHandler, IBool  {
		// This is for just a button press
		
		
		public bool bool_value = true;
		public int message = 0;
		
		
		
		public bool GetBool (){ return bool_value; }
		
		public int GetMessage (){ return (int)message; }
		
		public CanvasGroup group;
		public GameObject blocking;
		// maybe object too?
		
		
		public void StopCo (){
			
			StopAllCoroutines();
			
		}
		
		protected override void OnDisable(){
			base.OnDisable();
			StopCo();
			
		}
		
		
		public float alpha = 1f;
		
		protected DatesimVariables vars;
		protected override void OnEnable(){
			base.OnEnable();
			if (group == null) Debug.LogError("assign canvas group", gameObject); 
			
			
			if (vars == null) 
			{
				vars = GetComponentInParent<DatesimVariables>();
			}
			if (vars == null) {
				Debug.Log("no vars, this broke", gameObject);
				return; 
			}
			else
				vars.onChange += OnChange;
			
			
			// assume proximity is set elsewhere
			// assume app setting
			CheckAvailable();
			bool_value = vars.dateZone = available ;
			if (vars.dateZone) TurnOn();
			if (bool_value) alpha = 1;
			else alpha = 0;
			group.alpha = alpha;
			group.interactable = bool_value;
			group.blocksRaycasts = bool_value;
			
			
		}
		public void CheckAvailable(){
			available = (vars.power_on && vars.proximity && vars.app_on);
		}
		public bool available = true;
		protected virtual void OnChange(){
			if ( group == null ) return;
			CheckAvailable();
			
			if (available) 
			{
				if (vars.dateZone) TurnOn();
				return;
			}
			
			// respond like someone left the date zone
			
			TurnOff();
				
		}
		
		
		protected void Toggle() {
			if ( group == null ) return;
			
			
			bool_value = !bool_value;
			if (bool_value)
			{
				TurnOn();
			}
			else
			{
				TurnOff();
			}
			
			
		}
		protected override void OnPress() {
			
			base.OnPress();
			CheckAvailable();
			if (available) 
			
				Toggle();
			vars.OnChange();
			
			
		}
		protected void TurnOn() {
			bool_value = true;
			vars.dateZone = bool_value;
			
			StopCo();
			StartCoroutine("_TurnOn");
		}
		protected IEnumerator _TurnOn() {
			
			blocking.SetActive(true);
			while (alpha < 0.99f)
			
			{
				alpha += Time.deltaTime * .7f;
				
				group.alpha = alpha;
				
				yield return null;
			}
			alpha = 1f;
			group.alpha = alpha;
			group.interactable = true;
			group.blocksRaycasts = true;
			
			
		}
		
		protected void TurnOff() {
			bool_value = false;
			vars.dateZone = bool_value;
			StopCo();
			StartCoroutine("_TurnOff");
		}
		protected IEnumerator _TurnOff() {
			
			blocking.SetActive(false);
			group.interactable = false;
			group.blocksRaycasts = false;
			while (alpha > 0.01f)
			
			{
				alpha -= Time.deltaTime *1.1f;
				
				group.alpha = alpha;
				
				yield return null;
			}
			alpha = 0f;
			group.alpha = alpha;
			
			
		}
		
		

	}
}