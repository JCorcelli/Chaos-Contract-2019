using UnityEngine;
using System.Collections;

namespace SelectionSystem.Magnets
{
	
	public class MagnetDeploy : MonoBehaviour {

		// Use this for initialization
		protected IEnumerator Start () {
			yield return new WaitForSeconds(.2f);
			// maybe an instance would be better
			MagnetManager.Add(this.gameObject);
			
			
			GameObject.Destroy(this); // remove this script
		}
		
	}
}