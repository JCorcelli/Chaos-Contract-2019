using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using SelectionSystem;
namespace NPCSystem
{
	public class TriggerExitDropsIndicator : UpdateBehaviour {

		protected bool onExitRunning = false;
        public string _indicatorName = "TargetIndicator";
        public string _indicatorTag = "Magnets";
        public GameObject indicator; // this is a direction
		
		
        public string _targetName = "DesireIndicatorPresence";
        public string _targetTag = "Magnets";
        public Transform target; // this is a direction
		
		protected void Awake() {
			indicator = gameObject.FindNameXTag(_indicatorName, _indicatorTag);
			target = gameObject.FindNameXTag(_targetName, _targetTag).transform;
		}
		protected override void OnEnable() {
			base.OnEnable();
			
		}
		protected override void OnDisable() {
			base.OnDisable();
			if (hasControl)
				SelectGlobal.locked = false;
			
			onExitRunning = false;
		}
				
		protected bool hasControl = false;  // set true when locking
		
		protected override void OnLateUpdate(){
			
			if (onExitRunning)
				indicator.gameObject.GetComponent<NavMeshAgent>().Warp(target.position); // may need to 
		}
		protected IEnumerator OnTriggerExit(Collider col)
		{ 
			// something else is in control of the input if SelectGlobal.locked
			if (onExitRunning || SelectGlobal.locked) yield break;
			
			if (col.name == target.name)
			{
				yield return null;
				onExitRunning = true;
				//indicator.SendMessage("SetTarget", target);
				SelectGlobal.locked = true; // kills all selection actions
				hasControl = true;
				yield return new WaitForSeconds(0.1f);
				SelectGlobal.locked = false;
				//indicator.SendMessage("SetTarget", defaultTarget);
				onExitRunning = false;
			
			}
		}
		
		
	}
}