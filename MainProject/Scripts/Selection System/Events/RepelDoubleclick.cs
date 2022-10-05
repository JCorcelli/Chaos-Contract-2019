using UnityEngine;
using System.Collections;
using NPCSystem;

namespace SelectionSystem
{
	public class RepelDoubleclick : AbstractButtonHandler {

		public IRepellable target;
		public GameObject _target;
		public string targetName = "OnClickBunny";
		public string targetTag = "Player";
		
		protected void Awake () {
			if (_target != null)
				target = _target.GetComponent<IRepellable>();
			else
			{
				target = gameObject.FindNameXTag(targetName, targetTag).GetComponent<IRepellable>();
			}
			if (target == null) Debug.Log(name + " has no repel target",gameObject);
		}
		
		protected override void OnPress() {
			CheckDoubleclick();
		}

		
		public bool doubleclickRunning = false;
		public float doubleclickTimeout = .3f;
		
		protected void CheckDoubleclick() {
			if (doubleclickRunning)
			{
				StopCoroutine("DoubleclickTimer");
				doubleclickRunning = false;
				DoRepel();
			}
			else
				StartCoroutine("DoubleclickTimer");
		}
		protected IEnumerator DoubleclickTimer(){
			if (doubleclickRunning) 
				yield break;
			
			doubleclickRunning = true;
			yield return new WaitForSeconds(doubleclickTimeout);
			doubleclickRunning = false;
		}
		
		protected void DoRepel () {
			target.Repel();
				
		}
	}
}