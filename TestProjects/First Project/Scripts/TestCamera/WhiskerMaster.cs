using UnityEngine;
using System.Collections;

namespace TestProject.Cameras
{
	public class WhiskerMaster : MonoBehaviour {
		public Whiskers m_WhiskerSensor;
		private Whiskers[] aw;
		private bool clicked;
		// Use this for initialization
		void Start () {
			int count = 0;
			aw = new Whiskers[transform.childCount];
			foreach (Transform t in transform)
			{
				// adjust
				t.position -= t.forward * 5;
				Whiskers s = Instantiate(m_WhiskerSensor) as Whiskers;
				s.transform.parent = t;
				s.transform.localPosition = Vector3.zero;
				s.transform.localRotation = Quaternion.identity;
				aw[count] = s;
				count ++;
			}
			StartCoroutine("CheckGo");
		}
		private IEnumerator CheckGo(){
			yield return new WaitForSeconds(3f); // wait extra at init
			while (true)
			{
				enabled = true;
				if (clicked) 
				{
					yield return new WaitForSeconds(2f);
				}
				else
				{
					ResetTouch();
					yield return new WaitForSeconds(3f);
					if (!clicked) 
					{
					
						Whiskers ut = aw[0];
						CameraHolder.instance.CallTo(ut.transform);
						while (!clicked)
						{
							ut = Running(ut);
							yield return null;
						}
						CameraHolder.instance.Release(ut.transform);
					}
				}
			}
		}
		
		private Whiskers Running(Whiskers untouched){
			
			if (untouched.touched)
			{
				foreach (Whiskers w in aw)
				{
					if (!w.touched)
					{
						CameraHolder.instance.CallTo(w.transform);
						return w;
					}
					
				}
			}
			clicked = true;
			return untouched;
		}
		
		void Update(){
			
			clicked = Input.anyKey;
			enabled = !clicked;
		}
		public void ResetTouch(){
			foreach (Whiskers w in aw)
			{
				w.touched = false;
			}
			
		}
		
	}
}