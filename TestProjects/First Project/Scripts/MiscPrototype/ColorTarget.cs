using UnityEngine;
using System.Collections;

namespace TestProject
{
	public class ColorTarget : MonoBehaviour {
		public Material mat;
		
		
		void Start() {
			mat = GetComponent<Renderer>().material;
		}
	}
}