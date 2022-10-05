using UnityEngine;
using System.Collections;
using UnityEngine.Events;

using UnityEngine.UI;

namespace Utility
{
	public class TextMeshUpdate : UpdateBehaviour {

		// this'll be great for 3D debug / banner text
		
		protected TextMesh t;
		

		protected void Awake() { 
			t = gameObject.GetComponentInChildren<TextMesh>();
		}
		
		protected virtual void SetText(string newText) { 
			t.text = newText;
		}
		
	}

}