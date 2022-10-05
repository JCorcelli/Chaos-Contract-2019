
using UnityEngine;
using UnityEngine.Assertions;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using MEC;

using Utility.GUI;
using SelectionSystem;
using System.Text;
using static GuiGame.GuiGameVars;

namespace GuiGame
{


	public class GuiModDefault : MonoBehaviour{
		// see GuiMod for instructions
		// this component uses default rules
		
		public GuiRule rule = new DefaultRule();
		public void Run(Transform t, GuiDict d) => rule.Run(t,d);
		
	}
	
	
}