using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ActionSystem.OnActionScripts {


	public class ChangeLightToVisibility : MonoBehaviour, IOnAction {
		
		new protected Light light;
		public float intensityMax = 1f;
		public float intensityMin = .1f;
		
		public float delay = 0.5f;
		protected float currentLighting = 1f;
		
		void Start() { 
			light = GetComponent<Light>();
			currentLighting = light.intensity;
		}
		public void OnAction(ActionEventDetail data) {
			if (data.what == "Visible")
			{
				if (changing) StopCoroutine("DoChange");
				
				StartCoroutine("DoChange", intensityMax);
						
				
			}
			else if (data.what == "Hidden")
			{
				if (changing) StopCoroutine("DoChange");
				StartCoroutine("DoChange", intensityMin);
			}
		}
		
		protected bool changing = false;
		[Range(0f,20f)] public float ratePerSecond = 0.1f;
		protected IEnumerator DoChange(float goal){
			changing = true;
			if (changing) yield break;
			
			
			yield return new WaitForSeconds(delay);
			currentLighting = light.intensity;
			if (currentLighting < goal)
			{
				while (currentLighting < goal)
				{
					currentLighting += ratePerSecond * Time.deltaTime;
					light.intensity = currentLighting;
					yield return null;
				}
			}
			else
			{
				while (currentLighting > goal)
				{
					currentLighting -= ratePerSecond * Time.deltaTime;
					light.intensity = currentLighting;
					yield return null;
				}
			}
			
			currentLighting = goal;
			light.intensity = currentLighting;
			changing = false;
			
		}
		
	}
}