using UnityEngine;

using System.Collections;

namespace ActionSystem.OnActionScripts {

	public class ChangeMaterialWhileLocation : MonoBehaviour, IOnAction {
		
		public string locationName = "Bed";
		private bool touching = false;
		private Renderer rend;
		[SerializeField] private Material changedMaterial;
		private Material defaultMat;
		
		void Awake() { rend = GetComponent<Renderer>(); defaultMat = rend.material;}
		
		public void OnAction(ActionEventDetail data) {
			if (data.where.ToLower() == locationName.ToLower())
				
			{
				touching = true;
				int i =  0;
				while (i < rend.materials.Length) 
				{
					rend.materials[i] = changedMaterial;
					i ++ ;
				}
				
			}
			else if (touching)
			{
				
				touching = false;
				// change back
				int i =  0;
				while (i < rend.materials.Length) 
				{
					rend.materials[i] = defaultMat;
					i ++ ;
				}
			}
		
		}
	}
}