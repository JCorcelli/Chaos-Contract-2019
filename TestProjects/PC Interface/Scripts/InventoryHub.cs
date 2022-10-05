
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SelectionSystem;

namespace Inventory
{
	
	public class InventoryHub : UpdateBehaviour
	{
		
		public int banana = 0;
		public int cube = 0;
		
		protected void Start(){
			InventoryGlobal.items["banana"] = 0;
			InventoryGlobal.items["cube"] = 0;
			
		}
		protected override void OnUpdate(){
			banana = InventoryGlobal.items["banana"];
			cube = InventoryGlobal.items["cube"];
			
		}
		
	}
}