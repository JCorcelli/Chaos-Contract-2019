using UnityEngine;
using System.Collections;
using Utility.Managers;

namespace Utility.Triggers 
{
	public class TriggerHUBTogglesMesh : MonoBehaviour {

		public TriggerHUB hub;
		public bool onTrigger = true;
		
		protected MeshRenderer mesh;
		
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
				Debug.Log(name + " warning: no TriggerHub set in inspector",gameObject);
				return;
			}
			hub.onTriggerEnter += OnEnter;
			hub.onTriggerExit += OnExit;
			mesh = GetComponent<MeshRenderer>();
			
		}
		protected void Start () {
			if (hub.count < 1)
			mesh.enabled = !onTrigger;
			
		}
		protected void OnDestroy () { 
			if (hub == null) return;
			hub.onTriggerEnter -= OnEnter;
			hub.onTriggerExit -= OnExit;
		}
		
		protected void OnEnter(  ) {
			mesh.enabled = onTrigger;
		}
		protected void OnExit(  ) {
			mesh.enabled = !onTrigger;
			
		}
	}
}