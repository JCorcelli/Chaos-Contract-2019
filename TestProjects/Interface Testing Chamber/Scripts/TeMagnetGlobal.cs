using UnityEngine;
using System.Collections;
using SelectionSystem.Magnets;
namespace Utility
{
	public class TeMagnetGlobal : TextUpdate {

		
		// all about magnet global
		
		
		protected override void OnUpdate() {
			SetText(MagnetGlobal._canSwap + ">0?" +MagnetGlobal.canSwap);
		}
		
	}

}