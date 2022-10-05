using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	
	public class IHSCxHoverPhysic : IHSCxConnect {

		// it only targets the mouse, so technically I could do a manual check each frame based on the mouse instance
		protected CanvasGroup group;
		protected string mouseName = "MouseHotSpot";
		
		protected void Awake() {
			Connect();
			group = GetComponent<CanvasGroup>();
			group.blocksRaycasts = false;
			
		}
		
		
		protected override void OnEnable() {
			if (group != null) group.blocksRaycasts = false;
			
		}
		protected override void OnDisable() {
			if (group != null) group.blocksRaycasts = true;
			
		}
		
		
		protected void OnTriggerEnter2D(Collider2D caller) {
			if (ih.pressed) return;
			if (caller.name == mouseName)
			{
				if (ih != null) ih.Enter();
				if (group != null) group.blocksRaycasts = true;
			}
		}
		
		protected void OnTriggerExit2D(Collider2D caller) {
			
			if (caller.name == mouseName)
			{
				if (ih != null)  ih.Exit();
				if (group != null) group.blocksRaycasts = false;
			}
		}
		
		
	}
}