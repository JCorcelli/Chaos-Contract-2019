using UnityEngine;
using System.Collections;
using ActionSystem;

namespace NPCSystem
{
	public class JerriBedToFloor : RoutineNode {

		protected RoutineNode r_bed;
		public NavPingPong other;
		protected void Start () {
			r_bed = gameObject.FindNameXTag("JerriBedProcedure", "NPC").GetComponent<RoutineNode>();
			if (r_bed == null) Debug.Log(name + " does not have JerriBedProcedure",gameObject);
			
			// this is probably a child
			if (other == null)
				other = gameObject.FindNameXTag("JerriLead", "NPC").GetComponent<NavPingPong>();
			if (other == null) Debug.Log(name + " does not have JerriLead",gameObject);
		}
		
		
		protected bool _running = false; // personal accessor;
		
		
		
		public bool SetBusy(bool setting){
			busy = setting;
			if (other != null)
			{
				other.enabled = !busy;
				
				if (busy)
					other.agent.ResetPath();
			}
				
			return busy;
		}
		protected override void OnUpdate () {
			if (nextState == 0) state = 0;
				
			if (state == 0)
			{
				state = nextState;
				
			}
			
			if (state == 1) // bed
			{
				r_bed.nextState = 1;
				
				// prime bed
				state = 0;
				SetBusy(true);
				
				
			}
			else if (state == 2) // bed edge
			{
				r_bed.nextState = 2; // edge
				state = 0;
				SetBusy(true);
				
				
			}
			else if (state == 3) // get up
			{
				
				r_bed.nextState = 3; // edge
				if (r_bed.state > 1) 
					state = 4;
				SetBusy(true);
				
				
				
			}
			
			else if (state == 4)  // pacing
			{
				
				if (r_bed.state != EXITSTATE) return;
				state = 0;
				nextState = 0;
				SetBusy(false);
				//Countdown(JerriActions.GetAction(JerriActions.alert));
			}
			else if (state == 5)  // idle
			{
				state = 0;
				SetBusy(true);
			}
			else
			{
				state = 0; // some invalid nextState
			}
			
		}
		
		// submit an action, have it trigger on time for the event to end
		protected float delay = 
		.9f;
		
		protected void Countdown(ActionEventDetail data)
		{
			StartCoroutine("_Countdown", data);
		}
		protected IEnumerator _Countdown(ActionEventDetail data)
		{
			if (data == null) yield break;
			yield return null;
			if (delay - Time.deltaTime > 0f)
				yield return new WaitForSeconds(delay);
			
			while (ActionManager.Submit(data) == false) 
				yield return null; // will repeatedly try to submit until it works!
		}
		
	}
}

