using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SelectionSystem.Magnets
{
	
	public class MagnetManager : MonoBehaviour {

		public static MagnetManager instance;
		public static List<GameObject> adding = new List<GameObject>();
		
		
		public static void Add(GameObject[] g)
		{
			foreach (GameObject ob in g)
			{
				Add(ob);
			}
		}
		public static void Add(GameObject g)
		{
			// delay until instance exists
			adding.Add(g);
			if (instance != null)
			{
				instance.DelayAdd();
			}
			
			
		}
		protected bool addRunning = false;
		protected IEnumerator _DelayAdd() {
			if (addRunning) yield break;
			addRunning = true;
			yield return new WaitForSeconds(.1f);
			// adds on exist
			if (adding.Count > 0)
			{
				foreach (GameObject g in adding)
					g.transform.SetParent( instance.transform, true);
				
				adding.Clear();
			}
			
			addRunning = false;
		}
		
		
		protected void DelayAdd() {
			if (!addRunning)
				StartCoroutine("_DelayAdd");
			
		}
		protected void Start () {
			
			if (instance != null)
			{
				GameObject.Destroy(this);
				return;
			}
			
			
			if (GetComponentInParent<Canvas>() == null) Debug.Log(name + "Warning: magnetmanager: is intended to have a canvas ",gameObject);
			
			instance = this;
			DelayAdd();
			
			
		}
		
		
	}
}