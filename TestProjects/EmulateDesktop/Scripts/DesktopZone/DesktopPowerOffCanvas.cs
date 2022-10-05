
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class DesktopPowerOffCanvas : InZoneAbstract
	{
		// this is supposed to enable selection of things, or be a selectable
		
		public CanvasGroup group;
		public CanvasGroup desktopGroup;
		public float alpha = 1f;
		public bool available = true;
		
		public bool power_on = false;
		
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
			
			if (group == null) 
			{
				GetComponent<UnityEngine.UI.Image>().enabled = true;
				group = gameObject.AddComponent<CanvasGroup>();
				group.interactable = false;
				group.blocksRaycasts = false;
			
			}
			if (power_on) 
			{
				alpha = 0;
			}
			else
				alpha = 1;
			
			group.alpha = alpha;
			
			desktopGroup.interactable = power_on;
			desktopGroup.blocksRaycasts = power_on;
			
		}
		
		
		
			
		protected override void OnMessage(int channel, int msg){
			if (channel != 0) return;
			
			if (msg == 0) 
				TurnOff();
			else if (msg == 1) 
				TurnOn(); 
		}
		
		protected override void OnChange() {
		}
			
		
		protected void TurnOn() {
			power_on = true;
			StopCo();
			StartCoroutine("_TurnOn");
		}
		protected IEnumerator _TurnOff() {
			
			desktopGroup.interactable = false;
			desktopGroup.blocksRaycasts = false;
			
			
			while (alpha < 0.98f)
			
			{
				alpha += Time.deltaTime * .3f;
				
				group.alpha = alpha;
				
				yield return null;
			}
			
			alpha = 1f;
			group.alpha = alpha;
			
			
			desktopGroup.alpha = 0f;
			
		}
		
		protected void TurnOff() {
			power_on = false;
			StopCo();
			StartCoroutine("_TurnOff");
		}
		protected IEnumerator _TurnOn() {
			
			
			desktopGroup.alpha = 1f;
			
			while (alpha > 0.11f)
			
			{
				alpha -= Time.deltaTime *.3f;
				
				group.alpha = alpha;
				
				yield return null;
			}
			alpha = 0f;
			group.alpha = alpha;
			desktopGroup.interactable = true;
			desktopGroup.blocksRaycasts = true;
			
		}
	}
}