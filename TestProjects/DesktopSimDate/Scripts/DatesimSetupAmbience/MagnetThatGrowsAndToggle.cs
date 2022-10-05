using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using SelectionSystem;

namespace Datesim.Setup
{
	
	
	
    public class MagnetThatGrowsAndToggle : AbstractButtonHandler
    {
		// place a 2D object on screen and call a delegate
		
		public RectTransform target;
		public RectTransform rectTransform;
		
		protected Vector3 lastMousePosition = Vector3.zero;
		protected Vector3 startPosition = Vector3.zero;
		public float deltaSpeed = 5f;
		
		public Image image;
		public Collider2D col;
		
		protected override void OnEnable(){
			base.OnEnable();
			if(rectTransform == null) rectTransform = GetComponent<RectTransform>();
			if(target == null) target = rectTransform;
			
			if(col == null) col = GetComponent<Collider2D>();
			
			if(image == null) image = GetComponent<Image>();
			Cancel();
		}
		
		public bool toggle = false;
		public void Cancel(){
		toggle = true; Toggle();}
		public void Toggle(){
			toggle = !toggle;
			
			if (toggle)
			{
				image.enabled = col.enabled = growing = true;
				startPosition = lastMousePosition = Input.mousePosition;
				
				UseButton();
			}
			else
			{
				image.enabled = col.enabled = false;
				target.localScale = Vector3.one * .1f;
			}
		}
		public bool growing = false;
		public float growRate = .1f;
        protected override void OnUpdate()
		
		{
			base.OnUpdate();
			if (!toggle) return;
			if (Vector3.Distance(startPosition,lastMousePosition) > SelectGlobal.dragDistance)
			{
				growing = false;
			}
			
			if (growing && target.localScale.y < 1f)
			{
				float growth = Time.deltaTime * growRate;
				target.localScale += Vector3.one * growth;
			}
			UseButton();
		}
		
		
        protected void UseButton()
        {
			
			float scaleFactor = rectTransform.lossyScale.y;
			
			Vector3 currentPosition = Input.mousePosition;
			Vector3 delta = currentPosition-lastMousePosition;
			
			if (delta.magnitude > deltaSpeed ) 
				lastMousePosition += delta.normalized * deltaSpeed  * scaleFactor;
			else
				lastMousePosition = currentPosition;
			
			target.position = lastMousePosition ;
			
			
        }
    }
}
