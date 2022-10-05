using UnityEngine;
using System.Collections;

namespace TestProject.Cameras
{
public class CAOnStart : CameraAssistant {

		
		protected IEnumerator KeepCalling () {
			while (CameraHolder.instance == null) 
			{
				if (!enabled) yield break;
				yield return null;
			}
			
			if (m_snapTo) SnapTo();
			if (m_snap) Snap();
			if (m_call) Call();
			
		}
		protected override void OnEnable () {
			if (CameraHolder.instance == null) 
			{
				StartCoroutine("KeepCalling");
				return;
			}
			if (m_snapTo) SnapTo();
			if (m_snap) Snap();
			if (m_call) Call();
		}
		
	}
}