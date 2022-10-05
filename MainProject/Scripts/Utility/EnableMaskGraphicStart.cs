using System;
using UnityEngine;
using UnityEngine.UI;


namespace Utility
{
	
    public class EnableMaskGraphicStart : MonoBehaviour
    {

		
        protected void Start()
        {
			
			GetComponent<Mask>().enabled = true;;
        }
    }
}
