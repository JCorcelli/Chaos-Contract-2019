using UnityEngine;
using System.Collections;
using Utility.Managers;

namespace Utility.Triggers 
{
	public class TriggerHUBTogglesChild : MonoBehaviour {

		public TriggerHUB hub;
		public bool onTrigger = true;
		public GameObject child;
		/*
		if it's rigidbody it can have more than one collider triggered at once 
		// if it's any it can have more than one collider enter it at once, I need to keep count because of these pitfalls
		*/
		protected void Awake () { 
			if (hub == null) 
			{
				hub = GetComponentInParent<TriggerHUB>();
			}
				
			if (hub == null) 
			{
				Debug.Log("No HUB", gameObject);
				return;
			}
			if (child == null)
				child = transform.GetChild(0).gameObject;
			hub.onTriggerEnter += OnEnter;
			hub.onTriggerExit += OnExit;
		}
		
		protected void Start () {
			if (hub.count < 1)
			child.SetActive(!onTrigger);
			else
			child.SetActive(onTrigger);
			
		}
		protected void OnDestroy () { 
			if (hub == null) return;
			hub.onTriggerEnter -= OnEnter;
			hub.onTriggerExit -= OnExit;
		}
		
		
		protected void OnEnter(  ) {
			child.SetActive(onTrigger);
		}
		protected void OnExit(  ) {
			child.SetActive(!onTrigger);
			
		}
	}
}