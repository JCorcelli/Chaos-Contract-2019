using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MEC;
using GuiGame;
using static GuiGame.GuiGameVars;
namespace BehaviorTree
{

	//**** wrapper component ****//
	public class BehaviorTree : GuiMod{
		public BehaviorTree(){
			rule = new BTRule();
		}
		
		public override void Run(Transform t, GuiDict d) => rule.Run(t,d);
		
	}
	
	//**** define a rule set ****//
	public class BTRule : GuiRule {
		/*
			Modified rulebook for GUI
		*/
		public BTRule(){
			
			// add Mod rules
			//Add("*", Inside_aButton);
			
			// add Method rules
			// AddMethod(key, validate, called with mod);
		}
		
		public BTNode oldTree;
		public BTNode rootNode;
		public delegate void BTRuleDelegate();
		
		protected static Dictionary<string, BTRuleDelegate> leafDict = new Dictionary<string, BTRuleDelegate>(){
			["default"] = MakeBTNode,
			["node"] = MakeBTNode,
			
			// true/false instantly
			["bool"] = MakeBTBool,
			// delay enabling child
			["chain delay"] = MakeBTChainDelay,
			// enable/disable only
			["empty"] = MakeBTEmpty,
			["parallel"] = MakeBTParallel,
			["parallel sequence"] = MakeBTParallelSequence,
			["repeat"] = MakeBTRepeat,
			["return delay"] = MakeBTReturnDelay,
			["return if"] = MakeBTReturnIf,
			["select"] = MakeBTSelect,
			["sequence"] = MakeBTSequence,
			["sleep"] = MakeBTSleep,
			["wait"] = MakeBTWait,
			
			// update never, requires programming
			["update"] = MakeBTUpdateNode,
			// update on change, and sets child active / inactive
			["decor"] = MakeBTDecorPingBool,
			// update on change
			["ping"] = MakeBTPingBool,
			
			// use the lambda functionality
			["lambda"] = MakeBTLambda
		};
		
	
		public static GuiDict curParams;
		public static BTNode curNode;
		public static BTNode curParent;
		public static BTRule curRule;
		public static void CleanStatic(){
			curParams = null;
			curNode = null;
			curRule = null;
			curParent = null;
			
		}
		
		protected static int iresult;
		protected static void UseCurParam(string s, ref int r){
			
			if (int.TryParse (curParams[s]?.GetVar(), out iresult)) r = iresult;
		}
		protected static float fresult;
		protected static void UseCurParam(string s, ref float r){
			
			if (float.TryParse (curParams[s]?.GetVar(), out fresult)) r = fresult;
		}
		protected static bool bresult;
		protected static void UseCurParam(string s, ref bool r){
			
			if (bool.TryParse (curParams[s]?.GetVar(), out bresult)) r = bresult;
		}
		
		protected static void MakeBTNode(){
			BTNode newNode = new BTNode();
			curNode = newNode;
		}
		protected static void MakeBTEmpty(){
			BTEmpty newNode = new BTEmpty() ;
			curNode = newNode;
			
		}
		protected static void MakeBTBool(){
			
			BTBool newNode = new BTBool() ;
			curNode = newNode;
			if (curParams == null) return;
			
			UseCurParam( "reverse", ref newNode.reverse);
			if (curParams["fail"] != null)
				newNode.succeed = false;
			else UseCurParam( "succeed", ref newNode.succeed);
			
		}
		protected static void MakeBTChainDelay(){
			BTChainDelay newNode = new BTChainDelay() ;
			curNode = newNode;
			if (curParams == null) return;
			
			
			UseCurParam( "delay", ref newNode.delay);
			
		}
		protected static void MakeBTParallel(){
			BTParallel newNode = new BTParallel() ;
			curNode = newNode;
			if (curParams == null) return;
			
			UseCurParam( "max success", ref newNode.exitS);
			UseCurParam( "max fail", ref newNode.exitS);
			
			UseCurParam( "success disables child", ref newNode.successDisablesChild);
			UseCurParam( "failure disables child", ref newNode.failureDisablesChild);
			UseCurParam( "exit on total", ref newNode.exitOnTotal);
			UseCurParam( "reset", ref newNode.resetVars);
			
			
			
			
		}
		protected static void MakeBTParallelSequence(){
			BTParallelSequence newNode = new BTParallelSequence() ;
			curNode = newNode;
			if (curParams == null) return;
			
			UseCurParam( "max success", ref newNode.exitS);
			UseCurParam( "max fail", ref newNode.exitF);
			
			UseCurParam( "success disables child", ref newNode.successDisablesChild);
			UseCurParam( "reset on disable", ref newNode.resetOnDisable);
			UseCurParam( "fail disables child", ref newNode.failDisablesChild);
			UseCurParam( "success disables child", ref newNode.successDisablesChild);
			UseCurParam( "repeat failures", ref newNode.repeatFailures);
			UseCurParam( "loop reset", ref newNode.loopReset);
			UseCurParam( "wait", ref newNode.wait);
			UseCurParam( "wait time", ref newNode.waitTime);
			
		}
		
		protected static void MakeBTRepeat(){
			 BTRepeat newNode = new BTRepeat() ;
			curNode = newNode;
			if (curParams == null) return;
			
			UseCurParam  ("max fail",   ref newNode.exitF);
			UseCurParam  ("max success",ref newNode.exitS);
			UseCurParam("delay",      ref newNode.delay);
			
			
		}
		protected static void MakeBTReturnDelay(){
			
			 BTReturnDelay newNode = new BTReturnDelay() ;
			curNode = newNode;
			if (curParams == null) return;

			UseCurParam( "delay", ref newNode.delay);
			
		}
		protected static void MakeBTReturnIf(){
			
			 BTReturnIf newNode = new BTReturnIf() ;
			curNode = newNode;
			if (curParams == null) return;
			
			UseCurParam( "return success", ref newNode.returnSuccess);
			UseCurParam( "return fail", ref newNode.returnFail);
			
			
		}
		protected static void MakeBTSelect(){
			
			 BTSequence newNode = new BTSequence() ;
			curNode = newNode;
			if (curParams == null) return;
			

			UseCurParam( "reset on disable", ref newNode.resetOnDisable);
			UseCurParam( "max fail", ref newNode.exitF);
			UseCurParam( "max success", ref newNode.exitS);
			
			
			
		}
		protected static void MakeBTSequence(){
			
			 BTSequence newNode = new BTSequence() ;
			curNode = newNode;
			if (curParams == null) return;
			
			UseCurParam( "reset on disable", ref newNode.resetOnDisable);
			UseCurParam( "max fail", ref newNode.exitF);
			UseCurParam( "max success", ref newNode.exitS);
			
			
			
		}
		protected static void MakeBTSleep(){
			
			 BTSleep newNode = new BTSleep() ;
			curNode = newNode;
			if (curParams == null) return;

			UseCurParam( "delay", ref newNode.delay);
			
			
			
		}
		protected static void MakeBTWait(){
			
			 BTWait newNode = new BTWait() ;
			curNode = newNode;
			if (curParams == null) return;
			
			UseCurParam( "delay", ref newNode.delay);
			
		}
		protected static void MakeBTUpdateNode(){
			
			 BTUpdateNode newNode = new BTUpdateNode() ;

			curNode = newNode;
			if (curParams == null) return;
		}
		protected static void MakeBTDecorPingBool(){
			
			 BTDecorPingBool newNode = new BTDecorPingBool() ;
			curNode = newNode;
			if (curParams == null) return;
			
			UseCurParam( "truth", ref newNode.truth);
			
		}
		protected static void MakeBTPingBool(){
			
			 BTPingBool newNode = new BTPingBool() ;
			curNode = newNode;
			if (curParams == null) return;
			

			UseCurParam( "wanting", ref newNode.wanting);
			
		}
		protected static void MakeBTLambda(){
			
			BTLambda newNode = new BTLambda() ;
			curNode = newNode;
			newNode.rule = curRule;
			if (curParams == null) return;
			
			newNode.action = curParams.parent["action"];
			newNode.message = curParams["message"];
			
			newNode.lambda = curParams.parent[METHOD];
			
		}
			
		protected void SetParams(GuiDict d) {
			for (int i = 0 ; i < d.Count ; i++)
			{
				Debug.Log(d[i].key);
			}
		}
		protected void MakeNodes(GuiDict d) {
			BTNode save = curParent;
			
			for (int i = 0 ; i < d.Count ; i++)
			{
				if (d[i].key == "params") continue;
				if (d[i].key == METHOD) continue;
				curParams = d[i]["params"];
				MakeNode(d[i]); // main {a { b } }
				curParent = curNode;
				MakeNodes(d[i]);
				curParent = save;
			}
		}
		protected void MakeNode(GuiDict d) {
			string type = d.key;
			// params?
			//Debug.Log(type);
			if (moduleNames.Contains(type)) return;
			
			if (leafDict.ContainsKey(type))
				leafDict[type].Invoke();
			else
				leafDict["default"].Invoke();
			
			curNode.parent = curParent;
			curParent.children.Add(curNode);
		}
		List<string> moduleNames = new List<string>();
		protected void AddModule(GuiDict d) {
			string name = d.key;
			// params?
			moduleNames.Add(name);
		}
		protected void NewTree() {
			CleanStatic();
			curRule = this;
			oldTree = rootNode;
			rootNode = curParent = new BTEmpty();
			moduleNames.Clear();
			dict.Join(VARIABLE_OBJECT);
			
		}
		protected override void Run() {
			NewTree();
			
			
			// add modules
			for (int i = 0 ; i < dict.Count ; i++)
				AddModule(dict[i]);
			
			GuiDict d = dict["main"];
			
			if (d == null) return;
			
			// join nodes
			MakeNodes(d);
			rootNode.SetActive(true);
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