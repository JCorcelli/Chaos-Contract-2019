using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Utility.Tree
{
	public class SplashForSceneLoad : MonoBehaviour {

		// Use this for initialization
		
		protected Image img;
		void OnEnable () {
			img = GetComponent<Image>();
			img.enabled =  (!LoadingTree.additive) ;
		}
		
	}
}