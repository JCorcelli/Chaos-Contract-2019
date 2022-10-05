using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Utility.UI
{
	public class ImageGroupGenerator : ImageGroup {

		public GameObject copied;
		
		
		protected void Awake(){
			
			// assume children is 1
			if (transform.childCount != 1) Debug.LogError("Generator uses [1] child only! Has [" + transform.childCount + "].", gameObject);
			
			copied = transform.GetChild(0).gameObject;
			if (copied.activeSelf) Debug.LogError("Child needs to be disabled, copied code has run.", gameObject);
			
			int i = spriteList.Length;
			int s = transform.childCount;
			
			GameObject g;
			while (s < i)
			{
				g = GameObject.Instantiate(copied);
				g.GetComponent<RectTransform>().SetParent(copied.transform.parent, false);
				g.SetActive(true);
				s ++;
			}
			copied.SetActive(true);
			// I can't set them because I don't know when they're done.
		}
	}

}