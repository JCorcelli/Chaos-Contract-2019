
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class CloseZoneCanvasGroup : InZoneAbstract
	{
		// this is supposed to enable selection of things, or be a selectable
		
		public CanvasGroup group;
		public float alpha = 1f;
		public bool available = false;
		
		protected void CheckAvailable(){
			
			available = !hub.inFocus;
		}
		
		protected override void OnEnable() {
			
			base.OnEnable();
			
			Initialize();
			
		}
		
		protected void StopCo(){StopAllCoroutines();}
		
		protected override void OnDisable(){
			base.OnDisable();
			StopCo();
		}
		protected void Initialize(){
			
			if (group == null) group = gameObject.AddComponent<CanvasGroup>();
			CheckAvailable();
			if (available) 
			{
				alpha = 1;
			}
			else
				alpha = 0;
			
			group.alpha = alpha;
			group.interactable = available;
			group.blocksRaycasts = false;
			
		}
		
		
		
			
		protected override void OnChange() {
			
			base.OnChange();
			if (group == null) group = gameObject.AddComponent<CanvasGroup>();
			CheckAvailable();
			if (available) 
			{ TurnOn(); return;}
			else
				TurnOff();
		}
		
		protected void TurnOn() {
			
			//StopCo();
			//StartCoroutine("_TurnOn");
			alpha = 1f;
			group.alpha = alpha;
			group.interactable = true;
			group.blocksRaycasts = false;
		}
		protected IEnumerator _TurnOn() {
			
			
			while (alpha < 0.99f)
			
			{
				alpha += Time.deltaTime * .7f;
				
				group.alpha = alpha;
				
				yield return null;
			}
			alpha = 1f;
			group.alpha = alpha;
			group.interactable = true;
			group.blocksRaycasts = false;
			
			
		}
		
		protected void TurnOff() {
			
			//StopCo();
			//StartCoroutine("_TurnOff");
			group.interactable = false;
			group.blocksRaycasts = false;
			alpha = 0f;
			group.alpha = alpha;
		}
		protected IEnumerator _TurnOff() {
			
			
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