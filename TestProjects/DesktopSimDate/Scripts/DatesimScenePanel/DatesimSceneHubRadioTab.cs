using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Utility;

namespace Datesim.Scene
{
	public class DatesimSceneHubRadioTab : MonoBehaviour{

		public DatesimSceneHub hub;
		public int scene = 1;
		public GameObject radioObject;
		protected Canvas canvas;
		protected GraphicRaycaster raycaster;
		protected CanvasGroup canvasGroup;
		protected void OnEnable(){
			if (hub == null)
				hub = GetComponentInParent<DatesimSceneHub>();
			if (radioObject == null)
				radioObject = gameObject;
			
			canvas = AddCanvas.Util( radioObject.transform);
			raycaster = canvas.GetComponent<GraphicRaycaster>();
			if (canvasGroup == null) 	
				canvasGroup = radioObject.GetComponent<CanvasGroup>();
			if (canvasGroup == null) 	
				canvasGroup = radioObject.AddComponent<CanvasGroup>();
			if (hub == null)
				Debug.Log("no hub", gameObject);
			else
			{
				hub.onChange -= OnChange;
				hub.onChange += OnChange;
			}
			DisableCanvas();
		}
		public void EnableCanvas(){
			raycaster.enabled = 
			canvasGroup.interactable = 
			canvasGroup.blocksRaycasts = 
			canvas.enabled = true;
			canvasGroup.alpha = 1f;
			
		}
		public void DisableCanvas(){
			raycaster.enabled = canvasGroup.interactable = canvasGroup.blocksRaycasts = canvas.enabled = false;
			canvasGroup.alpha = 0f;
		
		}
		public virtual void OnChange(){
			if (hub.scene == scene)
			{
				EnableCanvas();
			}
			else if (canvas.enabled)
			{
				DisableCanvas();
			}
				
		}
	}
}