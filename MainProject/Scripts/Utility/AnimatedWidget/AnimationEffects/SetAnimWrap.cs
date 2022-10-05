using UnityEngine;
using System.Collections;


namespace Utility.AnimationEffects
{
	public class SetAnimWrap : MonoBehaviour {

		public Animation other;
		public WrapMode wrap = WrapMode.Default;
		void Awake() {
			if (other == null) 
			{
				other = GetComponent<Animation>();
				if (other == null) 
				{
					Debug.Log("no animation component" ,gameObject);
					return;
				}
			}
			other.wrapMode = wrap;
		}
	}
		
}