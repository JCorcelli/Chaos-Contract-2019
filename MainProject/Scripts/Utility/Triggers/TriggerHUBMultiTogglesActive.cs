using UnityEngine;
using System.Collections;
using Utility.Managers;

namespace Utility.Triggers 
{
	public class TriggerHUBMultiTogglesActive : MonoBehaviour {

		public TriggerHUB[] hubs;
		public bool onTrigger = true;
		public GameObject child;
		/*
		if it's rigidbody it can have more than one collider triggered at once 
		// if it's any it can have more than one collider enter it at once, I need to keep count because of these pitfalls
		*/
		protected void Awake () { 
				
			if (hubs.Length < 1) 
			{
				Debug.Log("No HUB, add hubs manually", gameObject);
				return;
			}
			foreach (TriggerHUB hub in hubs)
			{
				if (hub == null) continue;
				hub.onTriggerEnter += OnEnter;
				hub.onTriggerExit += OnExit;
			}
		}
		
		protected void Start () {
			foreach (TriggerHUB hub in hubs)
			{
				if (hub == null) continue;
				if (hub.count < 1)
					gameObject.SetActive(!onTrigger);
				else
				{
					gameObject.SetActive(onTrigger);
					break;
				}
			}
			
		}
		protected void OnDestroy () { 
		
			foreach (TriggerHUB hub in hubs)
			{
				if (hub == null) continue;

				hub.onTriggerEnter -= OnEnter;
				hub.onTriggerExit -= OnExit;
				
			}
		}
		
		
		protected void OnEnter(  ) {
			gameObject.SetActive(onTrigger);
		}
		protected void OnExit(  ) {
			foreach (TriggerHUB hub in hubs)
			{
				if (hub == null) continue;
				
				if (hub.count > 0)
					return;
			}
			gameObject.SetActive(!onTrigger);
			
		}
	}
}