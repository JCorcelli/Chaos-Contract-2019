//GuiMod.cs
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
	
	// For extending GuiRule
	
	/*
		
		GuiMod.cs by Joseph Corcelli
		
		
		
		
		Please note this is nowhere near ready, but I'm putting it out in public anyways
			
		Everything you see may look like a complicated program. 
		
		You're in the right place to get some C# work done.
		
		There are two classes that need to be modified working together.
	
	*/
	
	/** Instructions:
	Copy this file
	
	erase abstract keywords
	
	1 Rename GuiMod to GuiModName (inspector component)
	
	2 Rename NewRule to NewRuleName

	3 change
		DefaultRule to NewRuleName
	
		
	
	*/
	
	//How it works:
	/*
	
	1 This component is placed in a prefab. 
		component-wise logic activates
	
	2 "yes" man copies the prefab on build
	
	3 NewRule, applies definitions
		component update logic applies
		
	4 GUI callback logic 
	
	refer to GuiStartMenu for basic callback reference. 
	
	Note: There is already some redundancy. OnClick in "Yes" man calls a lambda. It requires a component with click detection like SelectAbstract to enable it.
	
	
	*/
	
	
	
	public abstract class GuiMod : MonoBehaviour{
		/*
		For interactivity inhert: 
		
		SelectAbstract - for basic mouse control combos
		
		AbstractButton - for button presses
		
		AbstractAny - for keyboard and/or mouse combos
		
		UpdateBehaviour implements OnPress, etc methods from a static SelectionManager
	
	
		*/
		
		
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
			
			See GuiRule for use case and default methods
			
			
			There should be Add, Replace, and Remove handling methods and GUI
			
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