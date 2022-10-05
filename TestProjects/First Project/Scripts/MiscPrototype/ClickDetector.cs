using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace TestProject
{
[RequireComponent (typeof (EventTrigger))]

	public class ClickDetector : MonoBehaviour {
		
		public int clicks = 0;
		
		
		void Start(){
			EventTrigger trigger = GetComponentInParent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerClick;
			
			// works. can't see it in the inspector though
			entry.callback.AddListener( (eventData) => { Clicked(); } );
			trigger.triggers.Add(entry);

		}
		public void Clicked(){
			clicks ++;
		}
		
	}
}