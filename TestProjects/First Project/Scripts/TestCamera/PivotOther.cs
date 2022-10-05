using UnityEngine;
using System.Collections;

namespace TestProject
{
	[RequireComponent (typeof (SeeTarget))]
	public class PivotOther : MonoBehaviour {
		public float force = 1f;
		public bool visible = false;
		public LayerMask activeLayer;
		public GameObject target;
		public string findByTag;
		public string findByName;
		private RaycastHit hit;
		private SeeTarget st;
		private int counter = 0;
		
		void Start () {
			
			if (!target)
			{
				if (findByTag != "")
					target = GameObject.FindWithTag(findByTag);
				else if (findByName != "")
					target = GameObject.Find(findByName);
				
				st = gameObject.GetComponent<SeeTarget>();
			}
		}
		
		// Update is called once per frame
		private IEnumerator EndFlare(){
			while (counter > 0)
			{
				yield return new WaitForSeconds(.1f);
				target.transform.LookAt(transform);
				target.transform.RotateAround(transform.position, transform.up, -90);
				counter --;
			}
			
		}
		void LateUpdate () {
		
			// ray = thisposition to playerposition;
			if (!target || !st.visible) 
			{
				enabled = false;
				return;
			}
			
			visible = false;
			if (Physics.Linecast(transform.position,  target.transform.position, out hit, activeLayer)) {
				if (hit.collider.tag == findByTag || hit.collider.name == findByName)
				{
					visible = true;
					return;
				}
				
			}
			else
			{
				visible = true;
				return;
			}
			
			if (visible)
			{
				// check distance
				enabled = false;
				counter = 10;
				StartCoroutine("EndFlare");
				return; // I can see the camera. no need to keep turning it
			}
			target.transform.LookAt(transform);
			target.transform.RotateAround(transform.position, transform.up, force * Time.deltaTime);
			
			
		}
		
		void OnTriggerEnter(Collider col){
			if (col.tag == "Player") enabled = true;
		}
	}
}