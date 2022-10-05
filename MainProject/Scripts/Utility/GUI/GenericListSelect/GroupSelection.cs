using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace Utility.GUI
{
	
	public class GroupSelection : MonoBehaviour
	{
		public bool dirty = false;
		public GameObject _selected = null;
		public GameObject selected {
			get{return _selected;}
			set{
				if (_selected != value) 
				{
					dirty = true;
				
					_selected = value;
				}
				
			}
		}
		
				
		public List<GameObject> selection = new List<GameObject>();
	}
		
}