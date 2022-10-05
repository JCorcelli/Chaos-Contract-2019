using UnityEngine;

using System.Collections;




namespace Utility.GUI
{
	
	public class GenericListNode : MonoBehaviour
	
	{
		public GenericListBuilder list; // connection
		public MenuHUBScripted menuHUB; // I can tell which menu is open from here?
		public IGenericList GetList(){
			return list as IGenericList;
		}
	}	
}