using UnityEngine;
using System.Collections;

namespace Utility.GUI
{
	public class GuiRotate : UpdateBehaviour {

		public float z_degreeDelta = 90f;
		protected RectTransform imageTransform;
		
		void Awake () {
			imageTransform = GetComponent<RectTransform>();
		}
		
		
		protected override void OnUpdate () {
			Vector3 rotation = imageTransform.localEulerAngles;
			imageTransform.localEulerAngles = rotation + new Vector3(0, 0, z_degreeDelta * Time.unscaledDeltaTime);
		}
	}
}