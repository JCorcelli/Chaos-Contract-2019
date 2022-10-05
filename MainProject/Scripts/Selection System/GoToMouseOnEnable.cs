using System;
using UnityEngine;


namespace SelectionSystem
{
	
    public class GoToMouseOnEnable : MonoBehaviour
    {
		
        public Vector3 offset = new Vector3(0f, 0f, 0f);

        private void OnEnable()
        {
            transform.position = Input.mousePosition + offset;
        }
    }
}
