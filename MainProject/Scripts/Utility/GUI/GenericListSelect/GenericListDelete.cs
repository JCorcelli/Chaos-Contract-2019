using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;


using SelectionSystem;
using SelectionSystem.IHSCx;

namespace Utility.GUI
{
	
	[RequireComponent (typeof(HSCxController))]
	public class GenericListDelete : IHSCxConnect
	{
		// demo / debug class with functions easily copied
		
		public GenericListBuilder builder;
		
		public int index = 0;
		
		protected GroupSelection group;

		protected override void OnEnable() {
			base.OnEnable();
			if (group == null) group = GetComponentInParent<GroupSelection>();
			if (group == null) Debug.Log("Looked for group, found nothing", gameObject);
			ih.onClick += OnClick;
			
		}
		protected override void OnDisable() {
			base.OnDisable();
			ih.onClick -= OnClick;
			
		}
		
		protected void GetSelected() {
			GameObject g = group.selected ;
			index =  g.transform.GetSiblingIndex();
			
		}
		protected void OnClick(HSCxController caller)
		
		{
			if (group.selected == null) return;
			// this is directly dependent on the builder class
			GetSelected();
			builder.Delete(index);
			
			
			
		}
		
		
	}
		
}