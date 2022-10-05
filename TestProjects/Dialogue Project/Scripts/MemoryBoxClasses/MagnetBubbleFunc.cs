using UnityEngine;
using System.Collections;
using CameraSystem;
//using SelectionSystem;
namespace DialogueSystem
{
	public class MagnetBubbleFunc : MagnetBubble {
		/*
			Move an image on screen, like a compass. also varies by edge and facing.
		*/
		protected DAction action ;
		public string userName = "";
		//public int fullScale =  100;
		
		public float inflateMult =  3;
		//public int edgeScale =  100;
		
		
		public GameObject indicator;
		public DLocalAction localAction;
		
		protected override void OnEnable(){
			base.OnEnable();
			
			localAction = new DLocalAction();
			
			DAction.Use(localAction.a);
			
			action = new DAction("user");
			
			action.Add(SetScale);
			
			action = new DAction("user.end");
			
			action.Add(Kill);
			
			if (indicator) indicator.SetActive(false);
			
		}
		public bool focused = false;
		protected override void OnUpdate(){
			if (inRegion && indicator && focused)
			{
				if (!indicator.activeSelf)
					indicator.SetActive(true);
			}
			
			else if (indicator.activeSelf)indicator.SetActive(false);
		}
		protected override void OnDisable(){
			base.OnDisable();
		}
		protected void ShowNoneDelay(){
			StopCoroutine("_ShowNoneDelay");
			StartCoroutine("_ShowNoneDelay");
		}
		protected IEnumerator _ShowNoneDelay(){
			yield return new WaitForSeconds(20f);
			Kill();
		}
		protected void ScaleCompass(float f){
			
			float rad = GetRad();
			
			
			imageTransform.position -= Vector3.up * rad * imageTransform.localScale.y;
			
			imageTransform.localScale = Vector3.one * f;
			
			
			imageTransform.position += Vector3.up * rad * f;
		}
		protected void Inflate(){
			focused = true;
			
			ScaleCompass(inflateMult);
			Live();
			StopCoroutine("_ShowNoneDelay");
		}
		protected void Deflate(){
			focused = false;
			ScaleCompass(1f);
			if (indicator) indicator.SetActive(false);
			
			ShowNoneDelay();
			// how about if it's deflated long enough it can vanish? ShowNone();
		}
		protected void SetScale(string vars){
			// this is called for every registered func. the vars matter.
			if (vars == userName)
				Inflate();
			else
				Deflate();
		}
		protected void Kill(string vars){
			// this is called for every registered func. the vars matter.
			if (vars == "" || vars == userName)
			{
				Deflate();
				Kill();
			}
			
		}
	}
}