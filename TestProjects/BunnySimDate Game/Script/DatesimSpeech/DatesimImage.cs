using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Datesim 
{
	
	public class DatesimImage : MonoBehaviour {
		public Sprite[] sprites = new Sprite[]{};
		
		protected void SetSprite(string s){
			foreach (Sprite sp in sprites)
			{
				if (sp.name == s)
				{
					GetComponent<Image>().sprite = sp;
					return;
				}
			}
		}
	}
}