
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Zone
{
	
	public class OnUseSetActive : AbstractButtonHandler
	{
		// this is a leaf of the zone hub
		public bool setActiveTo = false;
		public GameObject target;
		
		protected override void OnEnable(){
			base.OnEnable();
			if (target == null) target = gameObject;
		}
		protected override void OnPress(){
			target.SetActive(setActiveTo);
		}
		
	}
}