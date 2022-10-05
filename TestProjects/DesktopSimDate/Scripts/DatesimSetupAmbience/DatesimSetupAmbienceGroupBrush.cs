using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim.Setup
{
	public class DatesimSetupAmbienceGroupBrush : DatesimSetupAmbienceConnect {
		// This is a list of locations, I should be able to auto-generate them
		
		
		public int relationRequired = 0;
		
		protected override void OnEnable()
		{
			base.OnEnable();
		}
		
		protected override void OnDisable()
		{
			base.OnDisable();
			StopAllCoroutines();
		}
		
		
		protected Vector3 lastMousePos;
		protected float distanceDelta = 0f;
		public int next = 0;
		public float increment = .5f;
		public IEnumerator Pick(){
			
			bool holdingButton = true;
			while (holdingButton)
			{
			while (distanceDelta< increment)
			{
			
				lastMousePos = Input.mousePosition;
				
				yield return null;
				
				distanceDelta += Vector3.Distance(lastMousePos, Input.mousePosition);
				
					
				holdingButton = Input.GetButton("mouse 1");
				if (!holdingButton) break;
			}
			if (!holdingButton) break;
			while (distanceDelta > increment)
			{
				distanceDelta -= increment;
				lastMousePos = Vector3.MoveTowards(lastMousePos, Input.mousePosition, increment); // this assumes I'll draw a straight line?
				NextPick();
				// place pick()
			}
			}
		}
		public void NextPick(){
			
			Transform t = transform.GetChild(next);
			DatesimSetupAmbienceContentSelector con = t.GetComponent<DatesimSetupAmbienceContentSelector>();
			
			
			next = (int)Mathf.Repeat(next + 1, transform.childCount );
			
			con.SetDragged();
		}
			
		public void RandomPick(){
			
			int sel = (int)Random.Range(0, transform.childCount - .1f);
			
			Transform t = transform.GetChild(sel);
			DatesimSetupAmbienceContentSelector con = t.GetComponent<DatesimSetupAmbienceContentSelector>();
			
			con.SetDragged();
		}
		public void SetDragged() // start
		{
			
			Transform t = transform.GetChild(0);
			DatesimSetupAmbienceContentSelector con = t.GetComponent<DatesimSetupAmbienceContentSelector>();
			
			con.SetDragged();
			StartCoroutine("Pick");
		}
		
		public bool selected = false;

		protected override void OnUpdate(){
			base.OnUpdate();
			if (selected  ) 
			{
				if (ambienceHub.selected != gameObject ) 
					selected = false;
			
				else if (Input.GetButtonUp("mouse 1"))
					RandomPick();
				
					
			}
		}
		
		protected bool calling = false;
		protected override void OnChange()
		{
			if ( calling ) return;
		}
		protected override void OnPress()
		{
			selected = true;
			ambienceHub.selected = gameObject;
			SetDragged();
			ambienceHub.OnChange();
			calling = false;
		}
		
		
		
		

	}
}