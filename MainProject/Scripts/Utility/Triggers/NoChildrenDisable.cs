using UnityEngine;
using System.Collections;

namespace Utility.Triggers 
{
	public class NoChildrenDisable : UpdateBehaviour {

		// probably a recttransform
		public bool destroy = true;
		protected override void OnUpdate(){
			if (transform.childCount < 1)
			{
				if (destroy)
					GameObject.Destroy(gameObject);
				else
					gameObject.SetActive(false);
			}
		}
		
	}
}