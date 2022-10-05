
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


namespace Utility
{

	[CreateAssetMenu(fileName = "Prefab Array", menuName = "ScriptableObjects/PrefabArray", order = 1)]
	public class PrefabArray : ScriptableObject { 
	
		
		public Transform[] a;
		
		public bool Contains(string prefabname)
		{
			
			foreach (Transform t in a)
			{
				if (t.name == prefabname){ return true;}
			}
			
			return false;
		}
		public Transform GetPrefab(string prefabname)
		{
			
			foreach (Transform t in a)
			{
				if (t.name == prefabname){ return t;}
			}
			
			return a[0];
		}
		
		public Transform this[int index]
		{
			get => a[index];
			set {
				a[index] = value;
			}
		}		
	}
}