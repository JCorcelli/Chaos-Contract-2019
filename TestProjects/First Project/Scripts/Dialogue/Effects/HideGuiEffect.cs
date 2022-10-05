using UnityEngine;
using System.Collections;

namespace Effects
{
	public class HideGuiEffect : MonoBehaviour {
		[SerializeField] private GameObject canvas;
		

		public void HideGuis ()
		{ 
			canvas.SetActive( false );
		}
		
		public void HideGui (string s)
		{
			GameObject g = canvas.transform.Find(s).gameObject;
			if (g != null)
				g.SetActive(false) ;
			
		}
		
		
		public void ShowGuis ()
		{
			canvas.SetActive( true );
		}
		
		public void ShowGui (string s)
		{
			GameObject g = canvas.transform.Find(s).gameObject;
			if (g != null)
				g.SetActive(true) ;
		}
	}
}