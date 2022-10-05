using System;
using System.Collections;
using UnityEngine;


namespace Utility
{
	
    public class DeactivateChildAtStart : MonoBehaviour
    {
		public GameObject other;
		
        protected IEnumerator Start()
        {
			yield return null;
			if (other == null)
				transform.GetChild(0).gameObject.SetActive(false);
			else
				other.SetActive(false);
        }
    }
}
