using UnityEngine;
using System.Collections;
using ActionSystem.OnActionScripts;
using ActionSystem;

namespace StealthSystem {
	
	public class StealthCalculator : MonoBehaviour {

	
		public string actorName = "";
		public int covers = 	0;
		public int lights = 	0;
		public int invisibility = 0;
		public int noise = 0;
		public int visibility = 0;
		
		
		void Start() {
			StartCoroutine("InitialCalc");
		}
		protected IEnumerator InitialCalc()
		{
			yield return null;
			ActionEventDetail action = new ActionEventDetail();
		
			action.who = actorName;
			if (visibility < 1)
				action.what = "Hidden";
			else
				action.what = "Visible";
			ActionSystem.ActionManager.SubmitEffect(action);
			
			
		}
		public void Recalculate()
		{
			StartCoroutine("_Recalculate");
		}
		protected bool running = false;
		
		protected IEnumerator _Recalculate()
		{
			if (running) yield break;
			
			
			running = true;
			
			yield return null; // not more than once per frame
			
			int newVisibility = 0; // by default, it is perceived that the bunny is visible, this doesn't change what other calculations make
			
			if (invisibility < 1)
			{
				if (covers > 0) newVisibility --; // default, no cover. each cover cancels one noise.
				else if (lights > 0) newVisibility ++; // default, no light
			}
			// else
			// I'm invisible, who caree
		
			if (noise > 0)  newVisibility ++; // default, no noise
			
			ActionEventDetail action = new ActionEventDetail();
			if (visibility != newVisibility) // there's a change
			{
				visibility = newVisibility;
				action.who = actorName;
				if (visibility < 1)
					action.what = "Hidden";
				else
					action.what = "Visible";
				ActionSystem.ActionManager.Submit(action);
			}
			
			
			running = false;
			
			
		}
	}
}