using System;
using UnityEngine;


namespace Utility
{
	
    public class SetPositionAtStart2D : MonoBehaviour
    {

        public Vector2 offset = new Vector2(0f, 0f);
		
        protected void Start()
        {
			
			RectTransform t = GetComponent<RectTransform>();
			
            t.anchoredPosition = new Vector2(0,0) + offset;
        }
    }
}
