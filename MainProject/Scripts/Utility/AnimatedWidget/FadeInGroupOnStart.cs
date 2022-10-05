using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Utility {
	public class FadeInGroupOnStart : MonoBehaviour {

		protected CanvasGroup _canvasGroup;
		protected float currentAlpha = 1f;
		public float[] alphaTargets;
		public float[] times;
		[Range(0f,20f)] public float[] ratesPerSecond;
		
		void Awake () {
			
			_canvasGroup = GetComponent<CanvasGroup >();
			currentAlpha = _canvasGroup.alpha;
		}
		
		void OnEnable () {
			StartCoroutine("DoChange");
		}
		
		void OnDisable () {
			changing = false;
			StopCoroutine("DoChange");
		}
		
		protected bool changing = false;
		protected IEnumerator DoChange(){
			if (changing) yield break;
			changing = true;
			float goal;
			int count = 0;
			float prevTime = 0f;
			float prevTransition = 0f;
			float rate;
			
			foreach (float time in times)
			{
				yield return new WaitForSeconds(time - prevTime - prevTransition);
				
				prevTime = time;
				goal = alphaTargets[count];
				rate = ratesPerSecond[count];
				prevTransition = 0f;
				
				if (currentAlpha < goal)
				{
					while (currentAlpha < goal)
					{
						currentAlpha += rate * Time.deltaTime;
						prevTransition += Time.deltaTime;
						_canvasGroup.alpha = currentAlpha;
						yield return null;
					}
				}
				else
				{
					while (currentAlpha > goal)
					{
						currentAlpha -= rate * Time.deltaTime;
						prevTransition += Time.deltaTime;
						_canvasGroup.alpha= currentAlpha;
						yield return null;
					}
				}
				
				currentAlpha = goal;
				_canvasGroup.alpha = currentAlpha;
				count ++;
			}
			changing = false;
			
		}
	}
}