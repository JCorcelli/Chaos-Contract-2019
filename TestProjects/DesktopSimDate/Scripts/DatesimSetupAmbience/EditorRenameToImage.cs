using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Utility.UI
{
	[ExecuteInEditMode]
	public class EditorRenameToImage : MonoBehaviour {

		protected Image image;
		
		protected void Update(){
			if (image == null) image = GetComponent<Image>();
			gameObject.name = image.sprite.name.Substring(0, image.sprite.name.Length - 1).Replace("_"," ");
			GameObject.DestroyImmediate(this);
		}
		
	}

}