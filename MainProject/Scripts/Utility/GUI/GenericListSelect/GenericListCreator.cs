using UnityEngine;

using System.Collections;
using System.Collections.Generic;



namespace Utility.GUI
{
	
	public class GenericListCreator : MonoBehaviour
	
	{
		public IGenericList list; // connection
		public Transform nodeTransform;
		
		public Transform prefab;
		
		protected void Create() {
			Transform t;
			nodeTransform = t = Object.Instantiate(prefab);
			
			GenericListNode node = t.GetComponent<GenericListNode>();
			
			list = node.GetList() as IGenericList;
			// GenericListBuilder.instance = null;
			
			
		}
		
	}	
}