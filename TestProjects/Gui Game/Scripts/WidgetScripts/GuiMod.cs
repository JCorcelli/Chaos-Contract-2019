
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


	//**** declare a component ****//
	//**** wrapper component ****//
	
	// inheritance list from complicated to simple: SelectAbstract, AbstractButton..., AbstractAny..., UpdateBehaviour, MonoBehaviour
	
	public abstract class GuiMod : MonoBehaviour{
		
		public GuiMod(){
			rule = new DefaultRule();
		}
		
		public GuiRule rule;
		public virtual void Run(Transform t, GuiDict d) => rule.Run(t,d);
		public virtual void BuildWindow(Transform t, GuiDict d) => rule.BuildWindow(t,d);
		
	}
	
	//**** define a rule set ****//
	public abstract class NewRule : GuiRule {
		/*
			Modified rulebook for GUI
		*/
		public NewRule(){
			
			// add Mod rules
			//Add("*", Inside_aButton);
			
			// add Method rules
			// AddMethod(key, validate, called with mod);
		}
		
		protected override void Run() {
			// Add("(*)", TabMethod)
			
			// vars => dict.Fetch(VARIABLE_OBJECT)
			// globalVars => dict.Root[VARIABLE_OBJECT]
			// Fetch =>
			// code...
		}
		
		public void TabMethod() {
			// selected
		}
		/*
		
		INHERITANCE & RULESETS
		Define a class
			Add(string key, GuiRuleDelegate)
			
		This is helpful for reusing the class.
		
		*/
	}
	
}