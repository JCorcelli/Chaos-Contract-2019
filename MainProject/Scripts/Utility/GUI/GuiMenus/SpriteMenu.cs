using UnityEngine;
using System.Collections;


namespace Utility.GUI
{
	public class SpriteMenu : Menu {

		// Use this for initialization
		
		protected override void Awake () {
			_anim = GetComponent<Animator>();
			_canvasGroup = GetComponent<CanvasGroup>();
			
			
			if (IsOpen()) Debug.Log(gameObject.name + "Warn: If starting with the animation running was not intentional, set IsOpen to false.",gameObject);
		}
		
		
	}
}