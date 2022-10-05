using UnityEngine;
using System.Collections;
using ActionSystem.Subscribers;
using ActionSystem;

namespace NPCSystem
{
	public class RoutineBunnyInLocation : AbstractSubscriber {
		
		
		private ActionHandler npc;
		
		
		//Animator anim ;	
		public bool playerInput = true;
		public float patience = 8f;
		private float _ci_countdown;
		
		protected virtual void Start () {
			// I should know where the components are
			npc = GetComponent<ActionHandler>(); 
			//anim = GetComponentInChildren<Animator>();
			_ci_countdown = patience;
		}
		
		
		
		
		
		
		protected override void OnAction (ActionEventDetail data) {
			
			// 1. Check messages / keywords
			/// recognize this:  recent action is a condition determiner
			string action = data.what; // this is what happened
			
			
			if (action == "Entering"    ) 
			{
				//enabled = true;
				ReactLocation(data); 
			}
			else if (action == "Leaving")
				ReactLeaving(data);
			//	enabled = false;
			
			if (action == "SetVisible"    ) 
			{
				ReactStealth(data); 
			}
			
			
			
			
		}
		
		/// *** STEALTH***
		protected Coroutine hideTask;
		protected void HideTask() 	{ 
			npc.what = "Hiding" ;
			npc.why = "Need a place for cover.";
			Submit(new ActionEventDetail(npc));
		}
		
		protected void ReactStealth( ActionEventDetail data) {
			if (hideTask != null)
				StopCoroutine(hideTask);
			if (data.how.ToLower() == "visible") // and naturally I want to hide... maybe maybe not?
				hideTask = StartCoroutine("QueuedTask", new TaskDelegate(HideTask));
			
		}
		/// *** EXPLORATION***
		protected Coroutine exploreTask;
		protected void ExploreTask() 	{ 
			npc.what = "Exploring" ;
			npc.why = "This is exciting.";
			Submit(new ActionEventDetail(npc));
		}
		
		protected void ReactLeaving( ActionEventDetail data) {
			
			if (exploreTask != null)
				StopCoroutine(exploreTask);
			
			exploreTask = StartCoroutine("QueuedTask",  new TaskDelegate(ExploreTask));
			
		}
		
		/// *** SPECIFICS***
		protected void ReactLocation( ActionEventDetail data) {
			// location knowledge based reactions, technically the npc doesn't need to be there
			
			// if (aiBusy) what was he doing? I could conclude the previous action is why he's doing the current action.
			
			if (locationTask != null)
				StopCoroutine(locationTask);
			
			
				
			if (data.where.ToLower() == "bed")
			{
				locationTask = StartCoroutine("QueuedTask", new TaskDelegate(DigTask));
				
				// hidden
				// start a task that'll see if he can dig
				
				
					
			}
			else if (data.where.ToLower() == "cage")
			{
				locationTask = StartCoroutine("QueuedTask",  new TaskDelegate(SleepTask));
				
				// visible
				// eat
				// sleep
				// hide
				//
			}
			else if (data.where.ToLower() == "desk")
			{
				locationTask = StartCoroutine("QueuedTask",   new TaskDelegate(NapTask));
				
				// hidden
				// nap
				// 
				
				
			}
			else if (data.where.ToLower() == "chair")
			{
				locationTask = StartCoroutine("QueuedTask",  new TaskDelegate(NapTask));
				
				
				// hidden badly
				// nap
				//
				
			}
				
			
			
			
		}
		
		
		
		
		
		protected delegate void TaskDelegate();
		public bool aiBusy = false;
		public bool aiInterruptible = true;
		protected Coroutine locationTask;
		protected Coroutine location_delayedTask;
		
		protected IEnumerator NextFrameTask(TaskDelegate task){
			yield return null;
			if (task != null)
				task();
		}
			
			
		protected IEnumerator QueuedTask(TaskDelegate task){
			string triggerLocation = npc.where;
			
			while (true)
			{
				while (!aiBusy)
				{
					
					yield return null;
					// check if I'm in the right place, check if I'm too late
					if (npc.where != triggerLocation) yield break;
					else 
					{
						if (task != null) 
						{
							// check if the ai is able to act
							if (aiInput) 
							{
								aiBusy = true; 
								if (task != null)
									task();
							}
						}
					}
					
				}
				while (aiBusy)
				{
					yield return new WaitForSeconds(1f);
					if (npc.where != triggerLocation) yield break;
				}
			}
			
			
		}
		
		protected void DigTask() 		{ 
			npc.what = "Digging" ;
			npc.why = "There's a nice spot to dig.";
			Submit(new ActionEventDetail(npc));
		}
		protected void SleepTask() 		{ 
		
			npc.what = "Sleeping" ;
			npc.why = "Tired.";
			Submit(new ActionEventDetail(npc));
		}
		protected void NapTask() 		{ 
			npc.what = "Napping" ;
			npc.why = "Relaxed.";
			Submit(new ActionEventDetail(npc));
		}
		
		
		
		
		
		public bool aiInput = false;
		
		
		protected void AiPatienceTick(){
			
			
			if (Input.anyKey) playerInput = true; // i need to check if there's a player task situation
			else playerInput = false;
			
			
			if (!playerInput)
			{
				_ci_countdown -= Time.deltaTime;
				
				if (_ci_countdown <= 0)
				{
					aiInput = true;
				}
				
			}
			else if (aiBusy && playerInput)
			{
				if (aiInterruptible)
					aiBusy = false;
			}
			else
			{
				_ci_countdown = patience; // start at the time limit
				aiInput = false;
			}
					
					
		}
		
		void Update () {
				
			AiPatienceTick ();
			
			
		}
		
		
	}
		
}