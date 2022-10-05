using UnityEngine;
using System.Collections;


public delegate void GeneralDelegate();

namespace Utility.Managers
{
	public class GeneralUpdateManager : MonoBehaviour {

		// Use this for initialization
		public static GeneralDelegate onUpdate;
		public static GeneralDelegate onFixedUpdate;
		public static GeneralDelegate onLateUpdate;
		public static GeneralUpdateManager instance;
		
		void Awake() {
			if (instance == null) instance = this;
			else 
			{
				GameObject.Destroy(this);
				return;
			}
		}
		
		
		void FixedUpdate () {
			if (onFixedUpdate != null) onFixedUpdate();
		}
		void Update () {
			if (onUpdate != null) onUpdate();
		
		}
		void LateUpdate () {
			if (onLateUpdate != null) onLateUpdate();
		
		}
	}
}