using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CameraSystem
{ 
	/// <summary>
	/// Basic camera calling class
	/// </summary>
	public delegate void CADelegate();
	public enum CACallType { call = 0, snapTo = 1, snap = 2 }
	
	public class CameraAssistant : UpdateBehaviour 
	{ 
		public CACallType callType = CACallType.call;
		public CADelegate CallCamera;
		public int priority = 0;
		
		void Awake () { 
			if (callType == CACallType.snapTo) CallCamera = SnapTo;
			else if (callType == CACallType.call) CallCamera = Call;
			else //(callType == CallType.snap) 
				CallCamera = Snap;
		}

			
		// uses snapping
		public void SnapTo () { 
			if (CameraHolder.instance == null) return;
			transform.rotation = CameraHolder.instance.transform.rotation;
			transform.position = CameraHolder.instance.transform.position;
			CameraHolder.instance.CallTo(this);
		} 
		public void Snap () { // basically call, but the transform move instantly
			if (CameraHolder.instance == null) return;
			CameraHolder.instance.CallTo(this);
			CameraHolder.instance.SnapTo(this); // it's handled by holder
			
		}
		public void Call () { 	
			if (CameraHolder.instance == null) return;
			CameraHolder.instance.CallTo(this);
		}
		public void Release () { 
			if (CameraHolder.instance == null) return;
			CameraHolder.instance.Release(this);
		}
		
		
		
		
	}
}