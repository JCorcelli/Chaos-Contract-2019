using System;
using UnityEngine;
using System.Collections;

namespace Utility.Utils
{
	public class DestroyMesh : MonoBehaviour {
		private MeshRenderer mrender ; // The mesh renderer, only important in the editor
        private void Awake() {
            // Set up the reference.
            mrender = GetComponent<MeshRenderer>();
			
			
			// Hide while playing
			if (mrender != null)
				GameObject.Destroy(mrender);
			
			
		}
	}
}