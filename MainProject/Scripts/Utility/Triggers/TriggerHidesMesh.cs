using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

namespace Utility.Triggers 
{
	public class TriggerHidesMesh: MonoBehaviour {

		public string targetName = "PresenceIndicator";
		
		private MeshRenderer mrender;
		protected ShadowCastingMode castModeDefault;
		public ShadowCastingMode castModeWhenActive;
		public int count = 0; 
		/*
		if it's rigidbody it can have more than one collider triggered at once 
		// if it's any it can have more than one collider enter it at once, I need to keep count because of these pitfalls
		*/
		void Awake () { 
			mrender = GetComponent<MeshRenderer>();  
			
			castModeDefault = mrender.shadowCastingMode;
		}
		
		void OnTriggerEnter( Collider col ) {
			if (targetName == col.name) {
				count ++;
				
				mrender.shadowCastingMode = castModeWhenActive;
				
			}
		}
		void OnTriggerExit( Collider col ) {
			if (targetName == col.name) {
				
				count --;
				if (count <= 0)
				{
					count = 0;
					mrender.shadowCastingMode = castModeDefault;
				}
				
			}
			
		}
	}
}