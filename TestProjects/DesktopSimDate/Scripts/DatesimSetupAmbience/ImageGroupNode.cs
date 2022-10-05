using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Utility.UI
{
	public class ImageGroupNode : MonoBehaviour {

		public ImageGroup group;
		public int i ;
		public Image image ;
		
		
		protected void Awake(){
			
			group = GetComponentInParent<ImageGroup>();
			i = transform.parent.GetSiblingIndex();
			
			image = GetComponent<Image>();
			
			image.sprite = group.spriteList[i];
			name = group.spriteList[i].name.Replace("_"," ");
			
			GameObject.Destroy(this);
		}
	}

}