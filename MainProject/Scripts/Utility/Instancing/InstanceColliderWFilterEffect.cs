using UnityEngine;
using System.Collections;


namespace Utility
{
	public class InstanceColliderWFilterEffect : MonoBehaviour {

		// Use this for initialization
		
		public GameObject g;
		void Start () {
			
			Transform a = Instantiate(g).transform;
			a.parent = transform;
			a.localRotation = Quaternion.identity;
			a.localPosition = Vector3.zero;
			a.localScale = Vector3.one;
			
			GameObject ag = a.gameObject;
			MeshFilter agm = ag.GetComponent<MeshFilter>();
			agm.mesh = gameObject.GetComponent<MeshFilter>().mesh;
			ag.AddComponent<BoxCollider>();
		}
		
	}
}