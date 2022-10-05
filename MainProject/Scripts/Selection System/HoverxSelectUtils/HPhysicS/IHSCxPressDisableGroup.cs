using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	
	public class IHSCxPressDisableGroup : IHSCxConnect {

		// it only targets the mouse, so technically I could do a manual check each frame based on the mouse instance
		protected CanvasGroup group;
		protected string mouseName = "MouseHotSpot";
		
		protected void Awake() {
			group = GetComponent<CanvasGroup>();
			group.blocksRaycasts = false;
			
		}
		
		
		protected override void OnEnable() {
			Connect();
			if (ih == null) return;
			ih.onPress   += Press;
			ih.onRelease += Release;
			
		}
		protected override void OnDisable() {
			
			if (ih == null) return;
			
			ih.onPress   -= Press;
			ih.onRelease -= Release;
			
		}
		
		
		protected void Press(HSCxController caller) {
			
			if (group != null) group.blocksRaycasts = false;
		}
		
		protected void Release(HSCxController caller) {
			
			if (group != null) group.blocksRaycasts = true;
		}
		
		
	}
}