using UnityEngine;
using System.Collections;
using TestProject;

namespace TestProject.Cameras
{
	public class EagleVisor : MonoBehaviour {

	
		public Transform cameraArm;
		public Transform cs;
		
		public int gain	 = 0;
		public int stone = 0;
		public int lag	 = 0;
		public int interest = 0;
		[SerializeField] private float roundTimer = 1f; // seconds until pivot makes full circle, eg 360 in 1 second = 1f
		private SeeTarget _gain;
		private SeeTarget _stone;
		private SeeTarget _lag;
		private bool setTrigger = false;
		
		void OnDisable(){
			StopCoroutine("Recon");
			gain	= 0;
			stone 	= 0;
			lag	 	= 0;
		}
		void Start () {
			_gain = transform.Find("Pivot/Forward").gameObject.GetComponent<SeeTarget>();
			_lag = transform.Find("Pivot/Rear").gameObject.GetComponent<SeeTarget>();
			_stone = transform.Find("Pivot/Stone").gameObject.GetComponent<SeeTarget>();
			if (cameraArm == null) cameraArm = transform.Find("Camera");
		}
		void OnEnable(){
			StartCoroutine("Recon"); // never stops 
			}
		private IEnumerator Recon(){
			while (true)
			{
				// if interest reaches 2 I want the camera to be summoned to any place where I see gain without lag. That is the point of interest.
				if (gain < lag ) interest ++;
				else	
				{
					if (interest >= 2) CameraHolder.instance.Release(cameraArm.GetChild(0));
					interest = 0;
				}
				
				if (interest == 2) 
				{
					setTrigger = true;
				}
				
				gain	= 0;
				stone 	= 0;
				lag	 	= 0;
				
				yield return new WaitForSeconds(roundTimer);
			}
			
		}
		private void CallCam(){
			Ray ray = new Ray(_gain.target.transform.position, cs.position - _gain.target.transform.position);
			cameraArm.rotation = cs.rotation;
			
			// check if hitting wall
			cameraArm.position = ray.GetPoint(10);
			CameraHolder.instance.CallTo(cameraArm.GetChild(0));
			
			setTrigger = false;
		}
		
		// Update is called once per frame
		void LateUpdate () {
			if (_gain.visible) gain ++;
			if (_lag.visible)	lag ++;
			if (_stone.visible)	stone ++;
			
			if (setTrigger && _gain.visible && !_lag.visible)
				CallCam();
		}
	}
}