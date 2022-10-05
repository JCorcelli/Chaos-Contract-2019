using UnityEngine;
using System.Collections;


namespace Utility
{
	public class InstanceStencilR1Portal : MonoBehaviour {

		// Use this for initialization
		
		public GameObject g;
		IEnumerator Start () {
			
			yield return new WaitForSeconds(1f);
			
			Transform a = Instantiate(g).transform;
			a.parent = transform;
			a.localRotation = Quaternion.identity;
			a.localPosition = Vector3.zero;
			a.localScale = Vector3.one;
			a.gameObject.GetComponent<MeshFilter>().mesh = gameObject.GetComponent<MeshFilter>().mesh;
		}
		
	}
}