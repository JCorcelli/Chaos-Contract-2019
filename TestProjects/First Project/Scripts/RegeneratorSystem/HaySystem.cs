using UnityEngine;
using System.Collections;

namespace TestProject
{
	public class HaySystem : MonoBehaviour {

		// Use this for initialization
		public GameObject regenerator;
		public HayRegeneratorTarget target;
		
		// Update is called once per frame
		void Awake () {
			if (regenerator == null)
				regenerator = gameObject.GetComponentInChildren<HayRegenerator>().gameObject;
			if (target == null)
				target = gameObject.GetComponentInChildren<HayRegeneratorTarget>();
		}
		void Update () {
			if (target.size <= target.minSize) 
				regenerator.gameObject.SetActive(true);
		}
	}
}