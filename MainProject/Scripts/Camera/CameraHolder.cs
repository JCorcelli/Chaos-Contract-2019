using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CameraSystem
{
	public class CameraHolder : MonoBehaviour 
	{

		// This holds the camera.
		public static CameraHolder instance;
		
		public Transform targetA ;
		public Transform targetB ;
		
		
        public string targetAName = "PlayerFront";
        public string targetATag = "PlayerRig";
        public string targetBName = "PlayerCenter";
        public string targetBTag = "PlayerRig";
		
		protected Transform GetTarget (string name, string tag) {
			Transform target;
			
			
			target = gameObject.FindNameXTag(name, tag).transform;
			
			if (target == null)  Debug.Log(name + "no target " + name +" found.",gameObject);
			return target;
		}
		
		
		public bool locked = false;
		public CameraAssistant current;
		
		public SmoothTransform smoothTransform;
		
		protected List<CameraAssistant> recentCalls = new List<CameraAssistant>();
		
		public bool isInit = false;
		public void Init(){
			if (isInit) return;
			isInit = true;
			if (targetA == null) 
			{
				if (targetAName == "") Debug.LogError("Please enter a target A!");
				targetA = GetTarget(targetAName, targetATag);
			}
			
			if (targetB == null) 
			{
				if (targetBName == "") Debug.LogError("Please enter a target B!");
				targetB = GetTarget(targetBName, targetBTag);
			}
		}
		void Awake () {
			
			if (instance == null) instance = this;
			else
			{				
				GameObject.Destroy(this.gameObject);
				return;
			}
			Init();
			smoothTransform = GetComponent<SmoothTransform>();
			
			if (current != null)
			{
				recentCalls.Add(current);
				smoothTransform.target = current.transform;
			}
		}
		
		public bool delayForPlayerInput = true;
		protected void QueueTransition() {
			if (delayForPlayerInput)
			{
				if (Input.GetButton("mouse 1") || Input.GetButton("mouse 2") )
				{
					StartCoroutine("DelayCall");
					return;
				}
			}
			smoothTransform.target = current.transform;
			
		}
		
		public bool delayCallIsActive = false;
		protected IEnumerator DelayCall( ) {
			if (delayCallIsActive) yield break;
			delayCallIsActive = true;
			// getting input at random position isn't efficient
			while (Input.GetButton("mouse 1") || Input.GetButton("mouse 2") )
				yield return null;
			
			smoothTransform.target = current.transform;
			delayCallIsActive = false;
		}
		
		public void SnapTo(CameraAssistant other)
		{
			snapToThis = other;
			StartCoroutine("SnapCall");
		}
		
		protected bool snapping = false;
		protected CameraAssistant snapToThis;
		protected IEnumerator SnapCall(  ) {
			if (snapping) yield break;
			while (delayCallIsActive)
				yield return null;
			
			if (snapToThis != null)
			{
				transform.rotation = snapToThis.transform.rotation;
				transform.position = snapToThis.transform.position;
			}
			snapping = false;
		}
			
		public void CallTo(CameraAssistant other)
		{
			// asks camera
			// add new transform
			if (locked) return;
			if (!recentCalls.Contains(other)) recentCalls.Add(other);
			
			if (recentCalls.Count > 1)
				if (other.priority <= current.priority) return;
			
			
			current = other;
			if (snapToThis != other) snapToThis = null;
			QueueTransition();
		}
		public void Release(CameraAssistant other)
		{
			if (recentCalls.Contains(other)) {
				// remove
				recentCalls.Remove(other);
			}
			if (recentCalls.Count > 0) {
				// check a list
				
				if (current == other) 
				{
					if (snapToThis == other) snapToThis = null;
					
					current = recentCalls[recentCalls.Count - 1];
					
					foreach (CameraAssistant i in recentCalls)
					{
						if (i.priority >= current.priority) current = i;
					}
					
					QueueTransition();
				}
				
				
			}
			
				
		}
		
	}
}