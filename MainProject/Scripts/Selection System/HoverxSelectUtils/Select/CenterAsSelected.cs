using UnityEngine;
using System.Collections;

namespace SelectionSystem
{
	
		
	public class CenterAsSelected : SelectAbstract {
		// moves self until the selected child is in center of parent
		
		// only looks bad when scroll...
		// won't recognize select unless it's pressed?
	
		public bool horizontal = true;
		public bool vertical = false;
		
		public Transform center;
		public Transform t; // target
		public GameObject g; // target
		
		
		protected float speed = 235f;
		protected override void OnEnable(){
			base.OnEnable();
			
			if (center == null) center = transform.parent;
			if (center == null) enabled = false;
		}
		
		public bool focused = false;
		
		public void Select(Transform tr){
			t = tr;
			
			float f = speed;
			speed = 50000;
			SetPosition();
			speed = f;
			focused = false;
		}
		protected override void OnLateUpdate(){
			base.OnLateUpdate();
			if (Input.GetButtonDown("mouse 2") && isHovered)
				focused = false;
			else if (Input.anyKeyDown) 
				focused = true;
			
			
			
			// uiSelect?
			
			if (!focused)
			{
				t = null;
				return;
			}
			if (SelectGlobal.selectedTransform != null && SelectGlobal.selectedTransform.IsChildOf(transform)) 
			{
				
				t = SelectGlobal.selectedTransform;
				g = SelectGlobal.selected;
				
			}
			
			if (t == null 
			|| g == null 
			|| !g.activeSelf 
			|| !t.IsChildOf(transform) )
			{
				
				return;
			}
			
			
			SetPosition();
			
		}
		
		protected void SetPosition()
		{
			float diff;
			
			if (horizontal && vertical)
			{		
				diff = 	Mathf.Abs(Vector3.Distance(center.position, t.position));
					
				if (diff < speed *Time.deltaTime)
				{
					transform.position = center.position - t.position;
					
					focused = false;
					
				}
				else
				transform.position += (center.position - t.position).normalized *  speed *Time.deltaTime;
					
			}
			else if (horizontal)
			{		
				diff = (center.position.x - t.position.x);
				
					
				if (Mathf.Abs(diff) < speed *Time.deltaTime)
				{
					transform.position += Vector3.right * diff;
					
					focused = false;
				}
				else
				transform.position += Vector3.right * Mathf.Sign(diff) *  speed *Time.deltaTime;
					
					
			}
			else if (vertical)
			{		
				diff = (center.position.y - t.position.y);
				
					
				
				if (Mathf.Abs(diff) < speed *Time.deltaTime)
				{
					transform.position += Vector3.up * diff;
					
					
					focused = false;
				}
				else
				transform.position += Vector3.up * Mathf.Sign(diff) *  speed *Time.deltaTime;
					
			}
		}
	}
}