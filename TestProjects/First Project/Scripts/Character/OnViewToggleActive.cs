using UnityEngine;
using System.Collections;

using TestProject.Cameras;
public class OnViewToggleActive : MonoBehaviour {

	// Use this for initialization
		public Transform target;
		public GameObject child;
		protected GetFidelityToFront gff;
	void Start () {
		gff = gameObject.GetComponent<GetFidelityToFront>();
	
	}
	
	// Update is called once per frame
	void Update () {
		if (gff.Contains(target.position))
			child.SetActive( true );
		else
			child.SetActive( false );
	
	}
}
