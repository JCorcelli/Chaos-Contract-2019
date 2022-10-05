using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Utility
{
	public class AddCanvas : MonoBehaviour {
		/*
		protected Canvas canvas;
		protected GraphicRaycaster raycaster;
		protected CanvasGroup canvasGroup;
		
		public bool multistep = false;
		public int lessThan = 4;
		public int greaterThan = 0;
		public void EnableCanvas(){
			raycaster.enabled = canvasGroup.interactable = canvasGroup.blocksRaycasts = canvas.enabled = true;
			canvasGroup.alpha = 1f;
			
		}
		public void DisableCanvas(){
			raycaster.enabled = canvasGroup.interactable = canvasGroup.blocksRaycasts = canvas.enabled = false;
			canvasGroup.alpha = 0f;
		
		}
		public void RetrieveCanvas(){
			if (canvas != null) return;
			
			raycaster = gameObject.GetComponent<GraphicRaycaster>();
			canvasGroup = gameObject.GetComponent<CanvasGroup>();
		
		}*/
		
	
		
		public static void Fix(Canvas c){
			c.sortingOrder = c.transform.parent.GetComponentInParent<Canvas>().sortingOrder ;
		}
		public static Canvas Util(Transform t){
			
			Canvas c;
			c = t.gameObject.GetComponent<Canvas>();
			if (c == null) 
			{
				c = t.gameObject.AddComponent<Canvas>();
			}
			c.sortingOrder = t.parent.GetComponentInParent<Canvas>().sortingOrder ;
			
			if (t.gameObject.GetComponent<GraphicRaycaster>() == null)
			t.gameObject.AddComponent<GraphicRaycaster>();
			if (t.gameObject.GetComponent<CanvasGroup>() == null)
			t.gameObject.AddComponent<CanvasGroup>();
			
			return c;
		}
		protected void Awake () {
			Canvas c;


			c = gameObject.GetComponent<Canvas>();
			if (c == null)
				c = gameObject.AddComponent<Canvas>();
				
			c.sortingOrder = transform.parent.GetComponentInParent<Canvas>().sortingOrder ;
			
			//c.overrideSorting = true; // makes anything I want to cover this ugly
			
			if (gameObject.GetComponent<GraphicRaycaster>() == null)
			
				gameObject.AddComponent<GraphicRaycaster>();
			
		}
	}

}