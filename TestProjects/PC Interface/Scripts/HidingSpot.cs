using UnityEngine;
using System.Collections;
using SelectionSystem;
using CameraSystem;

namespace NPCSystem
{
	public class HidingSpot : MonoBehaviour {
		public HidingSpotHub hub;
		
		public string group = "1";
		public GameObject visual;
		
		protected void Awake(){
			if (hub == null) hub = GetComponentInParent<HidingSpotHub>();
			if (hub == null) {
				Debug.Log("no hub, this broke", gameObject);
				return; 
			}
			hub.onChange += OnChange;
			visual.SetActive(false);
			
		}
		public virtual void Show(){
			//visual.SetActive(false);
			if (hub.group == group) 
			{
				hub.group = "";
				hub.OnChange();
			}
		}
		public virtual void Hide(){
			//visual.SetActive(true);
			
			hub.group = group;
			hub.OnChange();
			
			
			
		}
		
		public virtual void OnChange(){
			if (hub.group == group)
			{
				visual.SetActive(true);
			}
			else
			{
				visual.SetActive(false);
			}
		}
	}
}