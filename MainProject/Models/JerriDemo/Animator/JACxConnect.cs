using UnityEngine;
using System.Collections;

namespace Anim.Jerri
{
	public class JACxConnect : UpdateBehaviour {

		
		public JerriAnimController controller {set{ ih = value; } get{ return ih;}
		}
		protected JerriAnimController ih;
		
		protected override void OnEnable(){
			base.OnEnable();
			Connect();
		}
		protected void Connect() {
				
			ih = gameObject.GetComponentInParent<JerriAnimController>();
			if (ih == null) Debug.LogError(name + " has no JerriAnimController", gameObject);
			
		}
		
		
	}
}