using UnityEngine;
using System.Collections;

namespace Anim.Jerri
{
	public abstract class JACxOnCall : JACxConnect {

		
		
		protected override void OnEnable(){
			Connect();
			if (ih == null) return;
			ih.onCall += OnCall;
		}
		
		protected override void OnDisable() {
			if (ih == null) return;
			ih.onCall -= OnCall;
		}
		protected virtual void OnCall(JerriAnimController caller) {}
		
	}
}