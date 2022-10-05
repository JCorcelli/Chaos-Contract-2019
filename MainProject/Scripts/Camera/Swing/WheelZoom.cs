using UnityEngine;
using System.Collections;

using SelectionSystem;
namespace CameraSystem.Swing
{
	public class WheelZoom	: UpdateBehaviour {
		
		protected override void OnUpdate(){
			//OnPress
			float h = Input.mouseScrollDelta.y * .1f;
			if (h == 0) return;
			Vector3 vec = transform.localPosition;
			float z = vec.z;
			z += h * 3;
			if (z >= -.5f) z = -.5f;
			else if (z <= - 6f) z = -6f;
			vec.z = z;
			transform.localPosition = vec;
			
		}
	}
	
}