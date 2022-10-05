using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TestProject.Cameras
{
	public class CameraAssistant : MonoBehaviour 
	{

		// Use this for initialization
		
		public string[] targetTags;
		public string[] targetNames;
		
		public bool m_snapTo= false;
		public bool m_snap  = false;
		public bool m_call  = false;
		
		private List<Transform> targets = new List<Transform>();
		public Transform target;
		
		
		protected virtual void Awake () {
			PopulateTargetsSelf();
		}
		protected virtual void Start () {}
		
		private void PopulateTargetsSelf () {
			if (targetTags.Length > 0)
			{
				// We're in business
				
				if (targetNames.Length > 0)
				{
					// findByTags and By Names
					foreach (string tt in targetTags)
					{
						foreach (GameObject ob in GameObject.FindGameObjectsWithTag(tt))
						{
							foreach (string tn in targetNames)
							if (ob.name == tn)
							{
								targets.Add(ob.transform);
								break;
							}
							
						}
					}
				}
				else
				{
					// findByTags
					foreach (string tt in targetTags)
					{
						foreach (GameObject ob in GameObject.FindGameObjectsWithTag(tt))
						{
							targets.Add(ob.transform);
							
						}
					}
				}
			}
			else if (targetNames.Length > 0)
				// warn, names
				Debug.Log ("Unity does not optimally search by name alone");
			
		}
		
		public void SnapTo () {
				transform.rotation = CameraHolder.instance.transform.rotation;
				transform.position = CameraHolder.instance.transform.position;
			} 
		public void Snap () {
			CameraHolder.instance.SnapTo(transform);
		}
		public void Call () {
			CameraHolder.instance.CallTo(transform);
		}
		
		protected virtual void OnEnable () {
			if (CameraHolder.instance == null) 
			{
				return;
			}
			if (m_snapTo) SnapTo();
			if (m_snap) Snap();
			if (m_call) Call();
		}
		protected virtual void OnDisable () {
			if (CameraHolder.instance == null) return;
			CameraHolder.instance.Release(transform);
			
		}
		
		
		protected void SetNearestTarget(ref Transform target){
			if (targets.Count <1 ) return;
			// if currentTarget isn't in range...
			
			float distance = 3000f;
			float nDist;
			foreach (Transform t in targets) {
				nDist = Vector3.Distance(t.position, transform.position);
				if (nDist < distance) {
					target = t;
					nDist = distance;
				}
			}
		}
		protected virtual void Update () {
				SetNearestTarget(ref target);
		}
	}
}