using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Utility.UI
{
	[ExecuteInEditMode]
	public class EditorFixBrushAnimation : MonoBehaviour {

		protected Image image;
		protected ImageBrushAnimation brush;
		protected void Update(){
			if (image == null) image = GetComponent<Image>();
			
			brush = GetComponent<ImageBrushAnimation>();
			if (brush == null) 
			{	
				GameObject.DestroyImmediate(this);
				
				return;
			}
			
			
			List<Sprite> sList = new List<Sprite>();
			List<string> strList = new List<string>();
			foreach (Sprite s in brush.list)
			{
				
				if (s.name.Substring(0, s.name.Length - 1) == image.name.Substring(0, image.name.Length - 1))
				{
					sList.Add(s);
					strList.Add(s.name);
				}
			}
			brush.list = sList.ToArray();
			
			System.Array.Sort(strList.ToArray(), brush.list);
			GameObject.DestroyImmediate(this);
				
				
				
		}
		
	}

}