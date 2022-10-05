using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupAmbienceObject : DatesimSetupAmbienceConnect {
		// This is a list of locations, I should be able to auto-generate them
		
		protected DatesimAmbienceRectUtil ar;
		
		public int relationRequired = 0;
		new public Collider2D collider;
		protected override void OnEnable()
		{
			base.OnEnable();
			
		
			if (collider == null) 
				collider = GetComponent<Collider2D>();
			
		
		
			
		}
		public bool isAlive = false;
		public bool isProxy = false;
		
		public virtual void MakeProxy(){
			isProxy = true;
		}
		public virtual void MakeReal(){
			enabled = true;
			// layer = 0;
			// animator.enabled = true;
			ar = gameObject.GetComponent<DatesimAmbienceRectUtil>();
			if (ar == null)
			ar = gameObject.AddComponent<DatesimAmbienceRectUtil>();
			
			if (collider == null) 
				collider = GetComponent<Collider2D>();
			isAlive = true;
			
			
			ambienceHub.Add(this);
			//collider.enabled = true;
			if (isAlive) ambienceHub.OnChange();
		}
		public override void OnDestroy() {
			base.OnDestroy();
			if (isProxy || ambienceHub == null) return;
			ambienceHub.Remove(this);
			ambienceHub.onChange -= OnChange;
			ambienceHub.OnChange();
			
		}
		public virtual void Kill(){
			isAlive = false;
			GameObject.Destroy(this.gameObject);
			
		}
		protected override void OnSelect(){
			//ambienceHub.AddSelection(this);
		}
		protected override void OnDeselect(){
			//ambienceHub.RemoveSelection(this);
		}
		protected override void OnChange()
		{
			// if "" == "" don't change it
			//if ((int)hub.vars.relation == access) return;
			
			//if (relationRequired > hub.access)
			//	GameObject.Destroy(gameObject);
			
			
		}
		
		
		
		

	}
}