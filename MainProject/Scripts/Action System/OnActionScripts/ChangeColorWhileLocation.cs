using UnityEngine;

using System.Collections;

namespace ActionSystem.OnActionScripts {

	public delegate void Func();
	public class ChangeColorWhileLocation : MonoBehaviour, IOnAction {
		
		
		public string locationName = "Bed";
		private bool touching = false;
		private Renderer rend;
		[SerializeField] private Material changedMaterial;
		private Color changedColor;
		private Color defaultColor;
		public bool changeAll = true;
		public bool lerpChange = true;
		public float rateOfChange = 0.1f;
		public string namedColor = "_Color";
		
		public Func onAction;
		
		void Awake() { 
			rend = GetComponent<Renderer>(); 
			defaultColor = rend.material.color;
			
			changedColor = changedMaterial.color;
			
			if (lerpChange) {
				if (changeAll)
					onAction = CoMultipleChange;
				else
					onAction = CoSingleChange;
			}
			else
			{
				if (changeAll)
					onAction = MultipleChange;
				else
					onAction = SingleChange;
			}
		}
		protected IEnumerator DoChange(int index){
			
			float interval = 0f;
			while (interval < 1f)
			{
				if (interval > 1f) interval = 1f;
				Color tempColor = Color.Lerp(defaultColor, changedColor, interval);
				
				rend.materials[index].SetColor( namedColor, tempColor);
				interval += rateOfChange * Time.deltaTime;
				yield return null;
			}
		}
		public void CoMultipleChange() {
			int i =  0;
			while (i < rend.materials.Length) 
			{
				
				StartCoroutine("DoChange", i);
				i ++ ;
			}
				
		}
		public void CoSingleChange() {
			StartCoroutine("DoChange", 0);
		}
			
			
		public void MultipleChange() {
			int i =  0;
			while (i < rend.materials.Length) 
			{
				rend.materials[i].SetColor( namedColor, changedColor);
				i ++ ;
			}
		}
		public void SingleChange() {
			rend.material.SetColor( namedColor, changedColor);
		}
		
		public void OnAction(ActionEventDetail data) {
			if (data.where.ToLower() == locationName.ToLower())
			{
				onAction();
				touching = true;
				
			}
			else if (touching)
				ChangeBack(); 
		
		}
		
		public void ChangeBack() {
			StopAllCoroutines();
			touching = false;
			// change back
			if (changeAll)
			{
				int i =  0;
				while (i < rend.materials.Length) 
				{
					rend.materials[i].SetColor( namedColor, defaultColor);
					i ++ ;
				}
			}
			else
				rend.material.SetColor( namedColor, defaultColor);
		}
	}
}