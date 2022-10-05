using UnityEngine;
using System.Collections;
using Utility.Managers;

namespace Utility.AnimatorEffects
{
	public class MecanimTriggerNode : TriggerNode {

		
		protected Animator anim;
		
		protected override void Awake () {
			base.Awake();
			anim = GetComponent<Animator>();
			if (anim == null) 
			{
				anim = GetComponentInChildren<Animator>();
				
				if (anim == null) 
					Debug.Log("needs Animator component",gameObject);
				
			}
		}
		
	}
}