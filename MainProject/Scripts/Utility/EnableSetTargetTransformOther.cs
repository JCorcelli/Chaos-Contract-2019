using UnityEngine;
using System.Collections;

namespace Utility
{
	public class EnableSetTargetTransformOther : MonoBehaviour {
		
		public Transform _target;
		protected ISetTargetTransform target;
		public Transform setToTransform;
		
		protected string targetName = "Const.Reaching";
		protected string targetTag = "NPC";
		
		protected void OnEnable(){
			if (target == null)
			{
				target = _target.GetComponent<ISetTargetTransform>();
			}
			
			target.SetTargetTransform(setToTransform);
			
			
			
		}
		
	}
}