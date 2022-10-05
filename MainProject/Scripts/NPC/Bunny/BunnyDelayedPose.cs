using UnityEngine;
using System.Collections;
using ActionSystem.Subscribers;


namespace NPCSystem
{
	public abstract class BunnyDelayedPose : OnActionSubscriber, IPosable {

		public string findName = "OnClickBunny";
		public string findTag  = "Player";
		public IPosable posable; 
		public IRepellable repellable; 
		public float delay = 1f;
		public int pose = 1;
		
		protected void Awake() {
			if (posable == null) 
				posable = gameObject.FindNameXTag(findName, findTag).GetComponent<IPosable>();
			if (repellable == null) 
				repellable = gameObject.FindNameXTag(findName, findTag).GetComponent<IRepellable>();
			
			if (posable == null) 
			{
				Debug.Log(name + "can't find " + findName,gameObject);
				enabled = false;
			}
		}
			
		
		public void SetPose(int p)
		{
			pose = p;
			SetPose();
			
		}
		protected void SetPose()
		{
			StopCoroutine("DelayedPose");
			
			
			if (pose > 0 && delay > 0.01f) StartCoroutine("DelayedPose");
			else
				posable.SetPose(pose);
		}
		public int GetPose() { return pose;}
		
		public void SetDelay(float f)
		{
			delay = f;
		}
		
		protected IEnumerator DelayedPose()
		{
			yield return new WaitForSeconds(delay);
			posable.SetPose(pose);
		}
	}
}