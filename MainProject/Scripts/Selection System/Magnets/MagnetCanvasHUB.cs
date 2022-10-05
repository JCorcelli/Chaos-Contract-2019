using UnityEngine;
using System.Collections;
using CameraSystem;

namespace SelectionSystem.Magnets
{
	
	public class MagnetCanvasHUB : UpdateBehaviour {

		// I might call MagnetView.SetScreenPoint
		// I might call MagnetView.SetWorldPoint
		
		// it is global, like the eventsystem. it has delegates.
		
		protected Canvas canvas;
		
		void Awake () {
			canvas = GetComponentInParent<Canvas>();
			
		}
		
		
		// Update is called once per frame
		protected override void OnUpdate () {
			canvas = GetComponentInParent<Canvas>();
			
		}
	}
}