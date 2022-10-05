using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace TestProject
{
[RequireComponent (typeof (EventTrigger))]

	public class BunnyPettable : MonoBehaviour {
		
		public int clicks = 0;
		
		public enum accessRights {restrict=0,allow=1};
		public accessRights accessRight = accessRights.allow;
		
		
		private BunnyPettingTarget target;
		
		
		
		void Start(){
			
			
			EventTrigger trigger = GetComponentInParent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerClick;
			
			// works. can't see it in the inspector though
			entry.callback.AddListener( (eventData) => { Clicked(); } );
			trigger.triggers.Add(entry);
			
			target = GameObject.FindObjectOfType(typeof(BunnyPettingTarget)) as BunnyPettingTarget;

		}
		public void Clicked(){
			clicks ++;
			if (accessRight == accessRights.allow)
			{
				target.clicks ++; // increment total clicks
				target.hitPosition(this.GetComponent<Collider>());
			}
			// bunny should jump away and warn you for harassment
		}
		
	}
}