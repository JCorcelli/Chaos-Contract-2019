using UnityEngine;
using System.Collections;
using Utility.Managers;

namespace Utility.AnimationEffects
{
	public class AnimTriggerNode : MonoBehaviour {

		
		public string managerName;
		public string managerTag = "Managers";
		public TriggerHUB manager;
		protected Animation anim;
		
		protected virtual void Awake () {
			
			if (manager == null)
			{
				GameObject managerObject = gameObject.FindNameXTag(managerName, managerTag);
				manager = managerObject.GetComponent<TriggerHUB>();
			}
			
			
			anim = GetComponent<Animation>();
			if (anim == null) 
			{
				anim = GetComponentInChildren<Animation>();
				
				if (anim == null) 
					Debug.Log("needs animation component",gameObject);
				
			}
		}
		
		protected void OnEnable() {
			manager.onTriggerEnter += onTriggerEnter;
			manager.onTriggerExit += onTriggerExit;
		}
		protected void OnDisable() {
			manager.onTriggerEnter -= onTriggerEnter;
			manager.onTriggerExit -= onTriggerExit;
		}
		
		
		protected virtual void onTriggerEnter () {
		
		}
		
		
		protected virtual void onTriggerExit () {
		
		}
	}
}