using UnityEngine;
using System.Collections;

namespace SelectionSystem.Magnets
{
	public class MagnetGlobal : UpdateBehaviour{

		public static int _canSwap = 0;
		
		public static bool canSwap {
			get{return _canSwap > 0;}
			set{
				if (value == true) _canSwap ++;
				else _canSwap --;
				if (_canSwap < 0) _canSwap = 0;
				}
		}
		
		// used for debugging
		
		public int cs = 0;
		public bool csf = false;
		protected override void OnUpdate() {
			csf = canSwap;
			cs = _canSwap;
		}
	}
}