using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SelectionSystem;

namespace CameraSystem.Swing
{
	public class CASwingMain : CameraAssistant 
	{

	
		public SwingArm current;
		public SwingArm defaultArm;
		public bool defaultOnExit = false;
		
		protected List<SwingArm> recentCalls = new List<SwingArm>();
		protected Transform pivot;
		protected Transform arm;
		
		protected IEnumerator Start () {
			arm = transform;
			pivot = arm.parent;
			
			
			SetWithDefault();
				
			yield return null;
			SyncToMain();
			patience = SelectionSystem.SelectGlobal.playerInputPatience;
			
		}
		public bool delayForPlayerInput = true;
		
		
		
		protected override void _OnPress() {
			if (Input.GetButtonDown("mouse 1"))
			{
				if (!Input.GetButtonDown("mouse 2")) desynced = true;
				
				if (delayForPlayerInput) 
				{
					timer = patience;
				}
			}
			
			else if (Input.GetButtonDown("mouse 2"))
			{
				
				timer = 0;
				if (desynced)
				{
					desynced = false;
					SyncToMain();
				}
			}
			
			
			
			
		}
		
		
		
		protected bool desynced = false;
			// until I make a global that lets me know. What this really means is something took camera away from [this].
		
		
		public float maxDegreesDelta = 90f;
		public float maxDistanceDelta = 10f;
		
		protected override void OnEnable() {
			base.OnEnable();
			CallCamera ();
			
			
			SelectionManager.onPress += _OnPress;
			
				
		}
		protected void SetDelayForPlayer(bool t) {
			
			
			
			if (!t) // I am subscribed, and patient, I need to negate it
			{
				
				timer = 0f;
				
			}
			
			
			// finally, set value
			delayForPlayerInput = t;
			
			
		}
		
		
		
		
		protected override void OnDisable () { 
			base.OnDisable();
			Release ();
			
		
			SelectionManager.onPress -= _OnPress;
			
		}
		
		
		protected float patience = 1.5f;
		public float timer = 0f;
		
		
		
		protected void SyncToMain(){
			Quaternion rotation = Quaternion.LookRotation( Camera.main.transform.forward);
			pivot.rotation = rotation;
				
			arm.position = Camera.main.transform.position;
			
			
		}
		
		protected override void OnUpdate(){
			
				
			if ((delayForPlayerInput && !timer.IsZero() && Input.GetButton("mouse 1")) || current == null) return;
			
			
			timer -= Time.unscaledDeltaTime;
			timer = Mathf.Clamp(timer, 0, patience);
				
			// adjust distance
			arm.localPosition = Vector3.MoveTowards(arm.localPosition, Vector3.Scale(current.transform.localPosition , current.transform.lossyScale), Time.unscaledDeltaTime * maxDistanceDelta );
			
			if (!timer.IsZero() ) return;
			// turn towards
			
			Quaternion rotation;
			
			rotation = current.transform.parent.localRotation;
			
			
			rotation = Quaternion.RotateTowards(pivot.localRotation, rotation, Time.unscaledDeltaTime * maxDegreesDelta ); // radians per second
				
			
			pivot.localRotation = rotation;
				
		}
		
		
		public void SetWithDefault()
		{
			
			if (defaultArm == null ) return;
				
			if(!recentCalls.Contains(defaultArm))
				recentCalls.Add(defaultArm);
			
			
			if (recentCalls.Count > 1) // A non-null disable will leave me with 1. Now I check priority.
			{
			if (current!= null && current != defaultArm && defaultArm.priority <= current.priority) return;
			}
			
			
			current = defaultArm;
			SetDelayForPlayer(current.delayForPlayerInput);
			
		}
		public void CallTo(SwingArm other)
		{
			// asks camera
			// add new transform
			
			
			if (!recentCalls.Contains(other)) recentCalls.Add(other);
			
			if (recentCalls.Count > 1) // A non-null disable will leave me with 1. Now I check priority.
			{
				if (current!= null && other.priority <= current.priority) return;
			}
			
			
			
			current = other;
			SetDelayForPlayer(current.delayForPlayerInput);
		}
		public void Release(SwingArm other)
		{
			if (recentCalls.Contains(other)) {
				// remove
				recentCalls.Remove(other);
			}
			if (recentCalls.Count > 0) {
				// check a list
				
				if (current == other) 
				{
					
					current = recentCalls[recentCalls.Count - 1];
					
					foreach (SwingArm i in recentCalls)
					{
						if (i.priority >= current.priority) 
							current = i;
						
					}
					SetDelayForPlayer(current.delayForPlayerInput);
					
				}
				
				
			}
			else if (defaultOnExit)
			{
				current = null;
				SetWithDefault();
			}
			
		}
		
	}
}