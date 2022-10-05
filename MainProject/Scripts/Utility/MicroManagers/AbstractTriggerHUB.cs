using UnityEngine;
using System.Collections;

namespace Utility.Managers
{
	public delegate void TriggerManagerDelegate();
	
	public abstract class AbstractTriggerHUB : MonoBehaviour {

		// Use this for initialization
		public TriggerManagerDelegate onTriggerEnter;
		public TriggerManagerDelegate onTriggerExit;
		
		
	}
}