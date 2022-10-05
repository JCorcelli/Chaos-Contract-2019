using System;
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace NPCSystem
{
    public class DesireDelayTrigger : MonoBehaviour
    {
		public string targetName = "DesireIndicatorPresence";
		public Transform target;
		public GameObject indicator ;
		new protected Transform transform;
		public float forceTime = 3f;
		public float cooldown = 2f;
		
		protected SphereCollider col;
		
		protected void Awake() {
			col = GetComponent<SphereCollider>();
			transform = GetComponent<Transform>();
			if (indicator == null) Debug.Log("need a manually added thing to offset",gameObject);
		}
		protected void OnTriggerEnter(Collider col){
			// remember to turn the collider off when not in use
			if (cooldownActive) return;
			if (col.gameObject.name == targetName) StartCoroutine("ForceOverTime");
				
		}
		protected void OnTriggerExit(Collider col){
			// remember to turn the collider off when not in use
			if (cooldownActive) return;
			if (col.gameObject.name == targetName)
			{
				StartCoroutine("RestCooldown");
			}
				
		}
		
		protected void OnDisable(){
			cooldownActive = false;
		}
		
		protected bool cooldownActive = false;
        protected IEnumerator RestCooldown(){
			if (cooldownActive) yield break;
			cooldownActive = true;
			yield return new WaitForSeconds(cooldown);
			cooldownActive = false;
        }
        protected IEnumerator ForceOverTime()
        {
			
			indicator.SendMessage("SetTarget", transform);
			indicator.transform.position = transform.position;
			
			yield return new WaitForSeconds(forceTime);
			
			indicator.SendMessage("SetTarget", target);
			
        }
		
		
    }
}
