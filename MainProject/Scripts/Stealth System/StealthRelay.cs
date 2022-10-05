using UnityEngine;
using System.Collections;
using ActionSystem;
using ActionSystem.Subscribers;

namespace StealthSystem {
	public class StealthRelay : OnActionSubscriber {

		// Use this for initialization
		
		public int covers
		{set {body.covers = value; body.Recalculate();}
		 get {return body.covers;}
		}
		public int lights
		{set {body.lights = value; body.Recalculate();}
		 get {return body.lights;}
		}
		public int visibility
		{set {body.visibility = value; body.Recalculate();}
		 get {return body.visibility;}
		}
		public int noise
		{set {body.noise = value; body.Recalculate();}
		 get {return body.noise;}
		}
		
		public StealthCalculator body;
		
		protected void Awake() {
			body = GetComponentInParent<StealthCalculator>();
			if (body == null) Debug.Log(name + "no body.",gameObject);
			
		}
		protected override void OnAction(ActionEventDetail act){
				if (act.receiver == gameObject)
				{
					if (act.what == "lights++")
					{
						lights++;
					}
					else if (act.what == "lights--")
					{
						lights--;
					}
				}
		}
	}
}