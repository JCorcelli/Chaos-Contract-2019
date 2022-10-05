using System;
using UnityEngine;


namespace Utility
{
	
    public class DeactivateAtStart : MonoBehaviour
    {

		
        protected void Start()
        {
			
			gameObject.SetActive(false);
        }
    }
}
