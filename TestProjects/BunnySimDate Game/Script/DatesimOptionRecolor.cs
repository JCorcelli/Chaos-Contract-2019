using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimOptionRecolor : DatesimAppConnect {
		// This is intended for big applications that need to coordinate actions
		
		
		public Color usedColor = Color.red;
		
		public Color failColor = new Color(.8f,.1f,.3f,1f);
		
		
		
		
		
		protected override void OnEnable( ){
			base.OnEnable();

			
			if (selectedImage == null) selectedImage = GetComponent<Image>();
			
			
			Connect();
		}
		
		public void AddColor() {
			
			if (vars.response_stage >= vars.stage ) 
			{
				FailAddColor();
				return;
			}
			selectedImage.color = usedColor;
			used = true;
		}
		
		public bool used = false;
		public void FailAddColor() {
			if (used) return;
			
			StopCoroutine("BackToWhite");
			if (gameObject.activeInHierarchy) StartCoroutine("BackToWhite");
		}
		
		protected override void OnDisable(){
			base.OnDisable();
			if (used || !running) return;
			
			StopCoroutine("BackToWhite");
			selectedImage.color = defaultColor;
			running = false;
		}
		public bool running = false;
		public float time = 0f;
		public float timeFraction = 0f;
		protected IEnumerator BackToWhite() {
			running = true;
			time = 0f;
			timeFraction = 0f;
			selectedImage.color = failColor;
			while (timeFraction < 1f)
			
			{
				time += Time.deltaTime;
				timeFraction = time / 3f;
				
				selectedImage.color = Color.Lerp(failColor, defaultColor, timeFraction);
				yield return null;
			}
			selectedImage.color = defaultColor;
			running = false;
		}
		
		
		public void Decolor(){
			selectedImage.color = defaultColor;
			callNumber = 0;
			used = false;
		}
		
		public static int callNumber = 0;
		
		// I can also have this check if the game's turned off.
		protected override void OnChange() {
			
			
				
			if (!vars.date_on)
			{
				Decolor(); // resets
			}
			
			if (vars.option == message) AddColor();
				
			
		}
		
		
		
	}
}