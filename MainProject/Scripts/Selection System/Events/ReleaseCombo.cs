using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
	public class ReleaseCombo : AbstractButtonComboPrecision
	{

		protected override void OnRelease(){
			Debug.Log("released " + buttonName,gameObject);
		}
	
	}
}