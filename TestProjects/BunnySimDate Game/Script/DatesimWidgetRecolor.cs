using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimWidgetRecolor : DatesimAppConnect {
		// This is intended for big applications that need to coordinate actions
		
		
		public Color activeColor = Color.red;
		public Color startColor = Color.white;
		public Color hoverColor = new Color(.8f,.1f,.3f,1f);
		
		
		
		
		
		protected override void OnEnable( ){
			base.OnEnable();
			
			
			
			Connect();
			lerpTime = 3f;
			AddColor(activeColor);
		}
		
		
		public void AddColor(Color c) {
			if (!gameObject.activeInHierarchy || selectedImage == null) return;
			selectedImage.color = c;
			StartCoroutine("BackToWhite");
		}
		public void StopCo (){
			
			StopCoroutine("BackToWhite");
			running = false;
		}
		
		protected override void OnDisable(){
			base.OnDisable();
			StopCo();
			
			if ( selectedImage == null) return;
			if (!isActive) selectedImage.color = defaultColor;
			running = false;
		}
		public bool running = false;
		public float time = 0f;
		public float timeFraction = 0f;
		public float lerpTime = 3f;
		protected IEnumerator BackToWhite() {
			if (running) yield break;
			startColor = selectedImage.color;
			running = true;
			time = 0f;
			timeFraction = 0f;
			
			while (timeFraction < 1f)
			
			{
				time += Time.deltaTime;
				timeFraction = time / lerpTime;
				
				selectedImage.color = Color.Lerp(startColor, defaultColor, timeFraction);
				
				yield return null;
			}
			selectedImage.color = defaultColor;
			running = false;
		}
		
		
		protected override void OnChange() {
			// ok this is for coloring things when stage changes
			if (vars.stage == 0 ) return;
				
			
			if (vars.stage == message)
			{
				lerpTime = 3f;
				StopCo ();
				AddColor(activeColor);
			}
			// every time something calls a message, in scope, this will check it
			
				
		}
		
		protected override void OnEnter(){
			if (isActive) return;
			
			SetColor(Color.white);
			
		}

		protected override void OnExit(){
			
			if (isActive) return;
			lerpTime = 1f;
			AddColor(hoverColor);
		}
		protected override void OnSelect(){
		}
		protected override void OnDeselect(){
		}
		
		protected void SetColor(Color c)
		{
			StopCo();
			selectedImage.color = c;
		}
		
	}
}