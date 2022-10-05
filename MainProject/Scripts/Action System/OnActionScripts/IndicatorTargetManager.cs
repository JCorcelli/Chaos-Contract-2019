using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using SelectionSystem;

namespace ActionSystem.OnActionScripts {
	public class IndicatorTargetManager : MonoBehaviour {

		// Use this for initialization
        public string _indicatorName;
        public string _indicatorTag;
        public GameObject indicator; // this is a direction
		
		public string _targetName;
		public string _targetTag;
		public Transform target; // this is a target
		
		
		public string _testAgainstName;
		public string _testAgainstTag;
		public Transform testAgainst; // this is a target
		
		public string _defaultTargetName;
		public string _defaultTargetTag;
		public Transform defaultTarget; // this is a target
		
		protected Transform best;
		void Start()
		{
			target = gameObject.FindNameXTag(_targetName, _targetTag).transform;
			indicator = gameObject.FindNameXTag(_indicatorName, _indicatorTag);
			testAgainst = gameObject.FindNameXTag(_testAgainstName, _testAgainstTag).transform;
			defaultTarget = gameObject.FindNameXTag(_defaultTargetName, _defaultTargetTag).transform;
			
			
		}
		
		protected float nearest = 10000f;
		protected bool queued = false;
		
		public void Query(GameObject g) {
			float distance = Vector3.Distance(g.transform.position, testAgainst.position);
			
			
			if (SelectGlobal.locked) return; // something else is in control of the target
			
			if (!queued)
			{			
				
				queued = true;
				StartCoroutine("TargetBestInQueue");
				nearest = distance;
				best = g.transform;
			}
			else
			{
				if (distance < nearest) 
				{
					nearest = distance;
					best = g.transform;
				}
			}
			
		}
		void OnDisable() {
			if (hasControl)
				SelectGlobal.locked = false;
		}
		protected bool hasControl = false; // set true when locking
		
		protected IEnumerator TargetBestInQueue() {
			// this will call and exit next frame
			
			yield return null;
			indicator.SendMessage("SetTarget", target);
			target.gameObject.GetComponent<NavMeshAgent>().Warp(best.position); // may need to be a "warp" command to navmesh agent
			best = null;
			nearest = 10000f;
			
			queued  = false;
			
			SelectGlobal.locked = true; // kills all selection actions
			hasControl = true;
			yield return new WaitForSeconds(1f);
			SelectGlobal.locked = false;
			
			indicator.SendMessage("SetTarget", defaultTarget);
			defaultTarget.gameObject.GetComponent<NavMeshAgent>().Warp( target.position ); // may need to be a "warp" command 
		}
		
	}
}