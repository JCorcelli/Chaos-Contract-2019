using System;
using UnityEngine;


namespace SelectionSystem.IHSCx
{
	
    public class OnEnablePressHSCx : IHSCxConnect
    {
		
        protected override void OnEnable()
        {
        
			Connect();
			if (ih == null) return;
			ih.Press();
        }
    }
}
