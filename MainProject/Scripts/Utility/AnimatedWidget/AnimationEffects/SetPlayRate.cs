using UnityEngine;
using System.Collections;

namespace Utility.AnimationEffects
{
	public class SetPlayRate : MonoBehaviour {


		protected Animation anim; 
		public float playbackSpeed = 1f;
		void Awake () { 
			anim = GetComponent<Animation>(); 
		
			anim[anim.clip.name].speed = playbackSpeed;
		}
		
	}
}