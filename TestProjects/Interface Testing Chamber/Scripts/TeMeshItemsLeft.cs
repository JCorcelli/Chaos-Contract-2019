using UnityEngine;
using System.Collections;


using SelectionSystem.IHSCx;

namespace Utility
{
	public class TeMeshItemsLeft : TextMeshUpdate {

		
		protected override void OnUpdate() { 
			SetText("" + ItemManager.holding);
		}
		
	}

}